using System.Collections.Generic;
using System.Linq;

namespace WorldRank
{
    public class Program
    {
        private static List<Player> players;
        public static void Main(string[] args)
        {
            players = new List<Player>();
            int choice;
            while ((choice = GetUserInput()) != 4)
            {
                switch (choice)
                {
                    case 1:
                        AddPlayer();
                        break;
                    case 2:
                        ListPlayers();
                        break;
                    case 3:
                        FindPlayerByName();
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }

        }
        private static int GetUserInput()
        {
            Console.WriteLine("1. Add Player");
            Console.WriteLine("2. List Players");
            Console.WriteLine("3. Find Player by Name");
            Console.WriteLine("4. Exit");
            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                return choice;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number between 1 and 4.");
                return GetUserInput();
            }
        }
        
        private static void AddPlayer()
        {
            Console.WriteLine("Enter player name:");
            string? name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Name cannot be empty. Please try again.");
                return;
            }
            players.Add(new Player(name));

        }
        private static void ListPlayers()
        {
            if (players.Count == 0)
            {
                Console.WriteLine("No players found.");
                return;
            }
            foreach (var player in players)
            {
                Console.WriteLine(player);
            }
        }
        private static void FindPlayerByName()
        {
            Console.WriteLine("Enter player name to search:");
            string? name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Name cannot be empty. Please try again.");
                return;
            }
            var foundPlayer = players.FirstOrDefault(p => p.Name == name);
            if (foundPlayer != null)
            {
                Console.WriteLine($"Found: {foundPlayer}");

            }
            else
            {
                Console.WriteLine("Player not found.");
            }
        }
    }
}