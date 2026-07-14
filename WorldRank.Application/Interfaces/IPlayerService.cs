using WorldRank.Domain.Entities;

namespace WorldRank.Application.Interfaces;

/// <summary>
/// Application use-cases for players. Pure orchestration over the repository; it has no
/// knowledge of how results are presented (console, web, ...).
/// </summary>
public interface IPlayerService
{
	Player AddPlayer(string name, int score);

	IReadOnlyList<Player> GetAllPlayers();

	IReadOnlyList<IGrouping<int, Player>> GroupPlayersByScore();

	Player? FindPlayerByName(string name);

	Player? FindPlayerById(int playerId);

	void DeletePlayer(int playerId);
}
