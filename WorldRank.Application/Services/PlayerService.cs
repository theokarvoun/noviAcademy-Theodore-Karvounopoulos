using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities;

namespace WorldRank.Application.Services;

/// <summary>
/// Application use-cases for players. Pure orchestration: it takes plain inputs,
/// talks to the repository and returns domain objects. It has no knowledge of how
/// the results are presented (console, web, etc.) - that is the delivery mechanism's job.
/// </summary>
public class PlayerService
{
	private readonly IPlayerRepository _playerRepository;

	public PlayerService(IPlayerRepository playerRepository)
	{
		_playerRepository = playerRepository;
	}

	public Player AddPlayer(string name, int score)
	{
		// Domain rules are enforced by the entity (throws on invalid name / negative score).
		var player = new Player(GeneratePlayerId(), name);
		player.AddScore(score);
		_playerRepository.AddPlayer(player);
		return player;
	}

	public IReadOnlyList<Player> GetAllPlayers()
	{
		return _playerRepository.GetAllPlayers().ToList();
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
		return _playerRepository.FindPlayer(playerId);
	}

	public void DeletePlayer(int playerId)
	{
		_playerRepository.DeletePlayer(playerId);
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
