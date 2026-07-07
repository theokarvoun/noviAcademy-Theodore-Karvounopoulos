using System.Collections.Generic;
using System.Linq;

namespace WorldRank
{
    public class Program
    {
        private static InMemoryPlayerRepository playerRepo;
        private static InMemoryWalletRepository walletRepo;
        private static int Id = 0;
        public static void Main(string[] args)
        {
            playerRepo = new InMemoryPlayerRepository();
            walletRepo = new InMemoryWalletRepository(playerRepo);
            int choice;
            while ((choice = GetUserInput()) != 8)
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
                        FindPlayerById();
                        break;
                    case 4:
                        DeletePlayer();
                        break;
                    case 5:
                        GroupByScore();
                        break;
                    case 6:
                        AddWalletToPlayer();
                        break;
                    case 7:
                        ListWalletsForPlayer();
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }

        }
        private static void GroupByScore()
        {
            var groupedPlayers = playerRepo.GroupPlayersByScore();
            foreach (var group in groupedPlayers)
            {
                Console.WriteLine($"Score: {group.Key}");
                foreach (var player in group)
                {
                    Console.WriteLine($" - {player}");
                }
            }
        }
        private static void AddWalletToPlayer()
        {
            Console.WriteLine("Give the player id to add wallet:");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Select currency:");
                Console.WriteLine("1. USD");
                Console.WriteLine("2. EUR");
                if (int.TryParse(Console.ReadLine(), out int currencyChoice))
                {
                    Wallet.Currency currency = currencyChoice switch
                    {
                        1 => Wallet.Currency.USD,
                        2 => Wallet.Currency.EUR,
                        _ => throw new ArgumentException("Invalid currency choice.")
                    };
                    Wallet newWallet = new Wallet(currency);
                    walletRepo.Add(newWallet, id);
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number for currency choice.");
                }
            }
            
        }
        private static void ListWalletsForPlayer()
        {
            Console.WriteLine("Enter player id to list wallets:");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var wallets = walletRepo.GetByPlayer(id);
                if (wallets.Count == 0)
                {
                    Console.WriteLine("No wallets found for this player.");
                }
                else
                {
                    foreach (var wallet in wallets)
                    {
                        Console.WriteLine(wallet);
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid player ID.");
            }
        }
        private static int GetUserInput()
        {
            Console.WriteLine("1. Add Player");
            Console.WriteLine("2. List Players");
            Console.WriteLine("3. Find Player by Name");
            Console.WriteLine("4. Remove Player");
            Console.WriteLine("5. Group Players by Score");
            Console.WriteLine("6. Add Wallet to Player");
            Console.WriteLine("7. List Wallets for Player");
            Console.WriteLine("8. Exit");
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
            playerRepo.AddPlayer(new Player(name,Id++));

        }
        private static void ListPlayers()
        {
            if (playerRepo.Players.Count == 0)
            {
                Console.WriteLine("No players found.");
                return;
            }
            foreach (var player in playerRepo.Players)
            {
                Console.WriteLine(player);
            }
        }
        private static void FindPlayerById()
        {
            Console.WriteLine("Enter player id to search:");
            if (!int.TryParse(Console.ReadLine(),out int id))
            {
                Console.WriteLine("Id cannot be empty. Please try again.");
                return;
            }
            var foundPlayer = playerRepo.FindPlayer(id);
            if (foundPlayer != null)
            {
                Console.WriteLine($"Found: {foundPlayer}");

            }
            else
            {
                Console.WriteLine("Player not found.");
            }
        }
        private static void DeletePlayer()
        {
            Console.WriteLine("Enter id for player to delete:");
            if (int.TryParse(Console.ReadLine(), out int playerId))
            {
                playerRepo.DeletePlayer(playerId);
                Console.WriteLine("Player deleted.");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid player ID.");
            }
        }

        }
}
