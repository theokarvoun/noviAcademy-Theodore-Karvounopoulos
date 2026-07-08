using System.Collections.Generic;
using System.Linq;

namespace WorldRank
{
    public class Program
    {
        private static IPlayerRepository? playerRepo;
        private static IWalletRepository? walletRepo;
        public static void Main()
        {
            playerRepo = new InMemoryPlayerRepository();
            walletRepo = new InMemoryWalletRepository(playerRepo);
            int choice;
            while ((choice = GetUserInput()) != 11)
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
                        AddPointsToPlayer();
                        break;
                    case 7:
                        ToggleWalletBlock();
                        break;
                    case 8:
                        AddWalletToPlayer();
                        break;
                    case 9:
                        ListWalletsForPlayer();
                        break;
                    case 10:
                        WithdrawOrDeposit();
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }

        }
        private static void AddPointsToPlayer()
        {
            Console.WriteLine("Enter player id to add points:");
            if (int.TryParse(Console.ReadLine(),out int id))
            {
                Console.WriteLine("Enter points to add:");
                if (int.TryParse(Console.ReadLine(), out int points))
                {
                    var player = playerRepo?.FindPlayer(id);
                    if (player != null)
                    {
                        player.AddScore(points);
                        Console.WriteLine($"Added {points} points to player {player.Name}. New score: {player.Score}");
                    }
                    else
                    {
                        Console.WriteLine("Player not found.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number for points.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid player ID.");
            }
        }
        private static void GroupByScore()
        {
            var groupedPlayers = playerRepo?.GroupPlayersByScore();
            if (groupedPlayers == null) return;
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
                    Currency currency = currencyChoice switch
                    {
                        1 => Currency.USD,
                        2 => Currency.EUR,
                        _ => throw new ArgumentException("Invalid currency choice.")
                    };
                    Wallet newWallet = new Wallet(currency);
                    walletRepo?.Add(newWallet, id);
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number for currency choice.");
                }
            }
            
        }
        private static List<IWallet>? ListWalletsForPlayer()
        {
            Console.WriteLine("Enter player id to list wallets:");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var wallets = walletRepo?.GetByPlayer(id);
                if (wallets == null) { 
                    Console.WriteLine("Player not found or no wallets available.");
                    return null;
                }
                if (wallets?.Count == 0)
                {
                    Console.WriteLine("No wallets found for this player.");
                    return null;
                }
                else if (wallets != null)
                {
                    int i = 0;
                    foreach (var wallet in wallets)
                    {
                        Console.WriteLine($"{++i}. {wallet}");
                    }
                    return wallets;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid player ID.");
            }
            return null;
        }
        private static void ToggleWalletBlock()
        {
            var wallets = ListWalletsForPlayer();
            if (wallets != null)
            {
                Console.WriteLine("Select wallet: ");
                if (int.TryParse(Console.ReadLine(),out int selection))
                {
                    if (--selection > wallets.Count())
                    {
                        Console.WriteLine("Invalid selection.");
                        return;
                    }
                    Console.WriteLine($"Selected wallet: {wallets[selection]}. Toggle block?[y/n]");
                    string? input = Console.ReadLine();
                    if (string.IsNullOrEmpty(input))
                    {
                        Console.WriteLine("Invalid input.");
                        return;
                    } 
                    if (input.Equals("y") && wallets[selection].IsBlocked)
                    {
                        wallets[selection].UnBlock();
                    } else if (input.Equals("y"))
                    {
                        wallets[selection].Block();
                    }
                }
            }
        }
        private static int GetUserInput()
        {
            Console.WriteLine("1. Add Player");
            Console.WriteLine("2. List Players");
            Console.WriteLine("3. Find Player by Id");
            Console.WriteLine("4. Remove Player");
            Console.WriteLine("5. Group Players by Score");
            Console.WriteLine("6. Add points to player");
            Console.WriteLine("7. Toggle player wallet block");
            Console.WriteLine("8. Add Wallet to Player");
            Console.WriteLine("9. List Wallets for Player");
            Console.WriteLine("10. Withdraw or Deposit");
            Console.WriteLine("11. Exit");
            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                return choice;
            }
            else
            {
                Console.WriteLine("Invalid input.");
                return GetUserInput();
            }
        }
        private static void WithdrawOrDeposit()
        {
            var wallets = ListWalletsForPlayer();
            Console.WriteLine("Select wallet: ");
            if (int.TryParse(Console.ReadLine(), out int selection))
            {
                if ((wallets != null) && (--selection < wallets.Count()))
                {
                    Console.WriteLine("1. Withdraw");
                    Console.WriteLine("2. Deposit");
                    if (int.TryParse(Console.ReadLine(),out int option))
                    {
                        try
                        {
                            switch (option)
                            {
                                case 1:
                                    Console.WriteLine("Amount to withdraw:");
                                    if (int.TryParse(Console.ReadLine(), out int value))
                                        wallets[selection].Withdraw(value);
                                    break;
                                case 2:
                                    Console.WriteLine("Amount to deposit:");
                                    if (int.TryParse(Console.ReadLine(), out int val))
                                        wallets[selection].Deposit(val);
                                    break;
                                default:
                                    Console.WriteLine("Invalid option.");
                                    break;
                            }
                        } catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
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
            playerRepo?.AddPlayer(new Player(name,playerRepo.GenerateId()));

        }
        private static void ListPlayers()
        {
            if (playerRepo == null)
            {
                Console.WriteLine("Player repository is not initialized.");
                return;
            }
            if (playerRepo?.GetAll().Count == 0)
            {
                Console.WriteLine("No players found.");
                return;
            }
            if (playerRepo?.GetAll() == null)
            {
                Console.WriteLine("Player list is null.");
                return;
            }
            foreach (var player in playerRepo.GetAll())
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
            var foundPlayer = playerRepo?.FindPlayer(id);
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
                playerRepo?.DeletePlayer(playerId);
                Console.WriteLine("Player deleted.");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid player ID.");
            }
        }

        }
}
