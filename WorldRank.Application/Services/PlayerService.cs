using Microsoft.Extensions.Logging;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities;

namespace WorldRank.Application.Services;

/// <summary>
/// Application use-cases for players. Pure orchestration: it takes plain inputs,
/// talks to the repository and returns domain objects. It has no knowledge of how
/// the results are presented (console, web, etc.) - that is the delivery mechanism's job.
///
/// Day 6: the service owns the cache. Reads are cache-aside (serve from cache, fall back to
/// the repository on a miss) and writes are write-through (update storage, then refresh the
/// cache and drop the stale list view).
/// </summary>
public class PlayerService : IPlayerService
{
	private static readonly TimeSpan Ttl = TimeSpan.FromSeconds(60);

	private readonly IPlayerRepository _playerRepository;
	private readonly ICache _cache;
	private readonly ILogger<PlayerService> _logger;

	public PlayerService(IPlayerRepository playerRepository, ICache cache, ILogger<PlayerService> logger)
	{
		_playerRepository = playerRepository;
		_cache = cache;
		_logger = logger;
	}

	private static string PlayerKey(int id) => $"player:{id}";
	private const string AllPlayersKey = "players:all";

	public Player AddPlayer(string name, int score)
	{
		// Domain rules are enforced by the entity (throws on invalid name / negative score).
		var player = new Player(GeneratePlayerId(), name);
		player.AddScore(score);
		_playerRepository.AddPlayer(player); // storage first

		_cache.Set(PlayerKey(player.Id), player, Ttl); // write-through
		_cache.Remove(AllPlayersKey);                  // invalidate the list view
		_logger.LogInformation("Cache write-through player {PlayerId}; list cache invalidated", player.Id);
		return player;
	}

	public IReadOnlyList<Player> GetAllPlayers()
	{
		if (_cache.TryGet(AllPlayersKey, out IReadOnlyList<Player>? cached) && cached is not null)
		{
			_logger.LogInformation("Cache HIT  all players");
			return cached;
		}

		_logger.LogInformation("Cache MISS all players — loading from repository");
		var players = _playerRepository.GetAllPlayers().ToList();
		_cache.Set(AllPlayersKey, (IReadOnlyList<Player>)players, Ttl);
		return players;
	}

	public IReadOnlyList<IGrouping<int, Player>> GroupPlayersByScore()
	{
		return _playerRepository.GroupPlayersByScore().ToList();
	}

	public Player? FindPlayerByName(string name)
	{
		return _playerRepository.GetAllPlayers()
			.FirstOrDefault(player => player.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
	}

	public Player? FindPlayerById(int playerId)
	{
		if (_cache.TryGet(PlayerKey(playerId), out Player? cached) && cached is not null)
		{
			_logger.LogInformation("Cache HIT  player {PlayerId}", playerId);
			return cached;
		}

		_logger.LogInformation("Cache MISS player {PlayerId} — loading from repository", playerId);
		var player = _playerRepository.FindPlayer(playerId);
		if (player is not null)
			_cache.Set(PlayerKey(playerId), player, Ttl);
		return player;
	}

	public void DeletePlayer(int playerId)
	{
		_playerRepository.DeletePlayer(playerId);

		_cache.Remove(PlayerKey(playerId)); // drop the entry
		_cache.Remove(AllPlayersKey);       // invalidate the list view
		_logger.LogInformation("Cache invalidated for deleted player {PlayerId}", playerId);
	}

	// Generates a random, unique player id (avoids collisions with already-registered players).
	private int GeneratePlayerId()
	{
		var existingIds = _playerRepository.GetAllPlayers().Select(player => player.Id).ToHashSet();

		int id;
		do
		{
			id = Random.Shared.Next(1, int.MaxValue);
		}
		while (existingIds.Contains(id));

		return id;
	}
}
