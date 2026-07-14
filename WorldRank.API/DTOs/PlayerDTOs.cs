using WorldRank.Domain.Entities;

namespace WorldRank.API.DTOs;

// Request DTO — what the client sends to create a player.
public record CreatePlayerRequest(string Name, int Score);

// Response DTO — what the client receives. Never expose the domain entity directly.
public record PlayerResponse(int Id, string Name, int Score)
{
	public static PlayerResponse From(Player player) =>
		new(player.Id, player.Name, player.Score);
}
