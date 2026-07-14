using WorldRank.Application.Interfaces;

namespace WorldRank.Console;

/// <summary>
/// Presentation handlers for the player menu. All console I/O lives here; the handlers
/// read input, call the application service, and render the result. The service itself
/// knows nothing about the console.
/// </summary>
public class PlayerMenu
{
	private readonly IPlayerService _playerService;

	public PlayerMenu(IPlayerService playerService)
	{
		_playerService = playerService;
	}

	public void AddPlayer()
	{
		System.Console.Write("Name: ");
		var name = System.Console.ReadLine();
		if (string.IsNullOrWhiteSpace(name))
		{
			System.Console.WriteLine("Name cannot be empty.");
			return;
		}

		System.Console.Write("Score: ");
		if (!int.TryParse(System.Console.ReadLine(), out var score))
		{
			System.Console.WriteLine("Score must be a whole number.");
			return;
		}

		_playerService.AddPlayer(name, score);
		System.Console.WriteLine("Player added successfully.");
	}

	public void ListPlayers()
	{
		var all = _playerService.GetAllPlayers();

		if (all.Count == 0)
		{
			System.Console.WriteLine("No players registered.");
			return;
		}

		foreach (var player in all)
			System.Console.WriteLine(player);
	}

	public void ListPlayersByScore()
	{
		var groups = _playerService.GroupPlayersByScore();

		if (groups.Count == 0)
		{
			System.Console.WriteLine("No players registered.");
			return;
		}

		foreach (var group in groups)
		{
			System.Console.WriteLine($"Score {group.Key}:");
			foreach (var player in group)
				System.Console.WriteLine($"  {player}");
		}
	}

	public void FindPlayerByName()
	{
		System.Console.Write("Search by name: ");
		var term = System.Console.ReadLine() ?? string.Empty;

		var player = _playerService.FindPlayerByName(term);

		System.Console.WriteLine(player is null ? "No player found." : player.ToString());
	}

	public void FindPlayerById()
	{
		var playerId = Prompts.PromptPlayerId();
		if (playerId is null)
			return;

		var player = _playerService.FindPlayerById(playerId.Value);

		System.Console.WriteLine(player is null ? "No player found." : player.ToString());
	}

	public void DeletePlayer()
	{
		var playerId = Prompts.PromptPlayerId();
		if (playerId is null)
			return;

		_playerService.DeletePlayer(playerId.Value);
		System.Console.WriteLine("Player deleted (if it existed).");
	}
}
