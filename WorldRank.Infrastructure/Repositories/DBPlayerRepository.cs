using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities;
using WorldRank.Infrastructure.Persistence.Context;

namespace WorldRank.Infrastructure.Repositories;

/// <summary>
/// Entity Framework Core backed implementation of <see cref="IPlayerRepository"/>.
/// Mirrors the behaviour of <see cref="InMemoryPlayerRepository"/> but persists to a database.
/// </summary>
public class DBPlayerRepository : IPlayerRepository
{
	private readonly WorldRankDBContext _context;
	private readonly ILogger<DBPlayerRepository> _logger;

	public DBPlayerRepository(WorldRankDBContext context, ILogger<DBPlayerRepository> logger)
	{
		_context = context;
		_logger = logger;
	}

	public void AddPlayer(Player player)
	{
		_context.Players.Add(player);
		_context.SaveChanges();
		_logger.LogInformation("Player {PlayerId} ({Name}) added with score {Score}", player.Id, player.Name, player.Score);
	}

	public IEnumerable<Player> GetAllPlayers()
	{
		// AsNoTracking: read-only listing, no change tracking overhead.
		return _context.Players.AsNoTracking().ToList();
	}

	public void DeletePlayer(int playerId)
	{
		var player = _context.Players.FirstOrDefault(item => item.Id == playerId);

		if (player is null)
		{
			_logger.LogWarning("Delete skipped: player {PlayerId} not found", playerId);
			return;
		}

		_context.Players.Remove(player);
		_context.SaveChanges();
		_logger.LogInformation("Player {PlayerId} deleted", playerId);
	}

	public Player? FindPlayer(int playerId)
	{
		return _context.Players.FirstOrDefault(item => item.Id == playerId);
	}

	public IEnumerable<IGrouping<int, Player>> GroupPlayersByScore()
	{
		// Materialise first, then group in memory so the grouping semantics match
		// InMemoryPlayerRepository exactly (returns full Player objects per group).
		return _context.Players
			.AsNoTracking()
			.ToList()
			.GroupBy(player => player.Score)
			.OrderByDescending(group => group.Key);
	}
}
