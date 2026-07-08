using NLog;
using System.Collections.Generic;
using System.Linq;
using WorldRank.Interfaces;
using WorldRank.Repository;

namespace WorldRank
{
    public class Program
    {
        private static IPlayerRepository? playerRepo;
        private static IWalletRepository? walletRepo;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static void Main()
        {
            logger.Info("Application starting.");
            // Global unhandled exception logging
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                logger.Fatal(e.ExceptionObject as Exception, "Unhandled domain exception");
            };
            System.Threading.Tasks.TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                logger.Fatal(e.Exception, "Unobserved task exception");
                e.SetObserved();
            };

            playerRepo = new InMemoryPlayerRepository();
            logger.Info("Player repository initialized.");
            walletRepo = new InMemoryWalletRepository(playerRepo);
            logger.Info("Wallet repository initialized.");

            try
            {
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
                            try
                            {
                                AddWalletToPlayer();
                            }
                            catch (Exception e)
                            {
                                logger.Error(e, "Failed to add wallet to player.");
                            }
                            break;
                        case 9:
                            ListWalletsForPlayer();
                            break;
                        case 10:
                            WithdrawOrDeposit();
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            logger.Warn("User entered invalid menu choice.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Unhandled exception in main loop.");
            }
            finally
            {
                logger.Info("Application shutting down.");
                LogManager.Shutdown();
            }
        }
        private static void AddPointsToPlayer()
        {
            Console.WriteLine("Enter player id to add points:");
            if (int.TryParse(Console.ReadLine(),out int id))
            {
                var player = playerRepo?.FindPlayer(id);
                if (player == null)
                {
                    //Console.WriteLine("Player not found.");
                    logger.Error("Player not found.");
                    return;
                }
                Console.WriteLine("Enter points to add:");
                if (int.TryParse(Console.ReadLine(), out int points))
                {

                    try
                    {
                        player.AddScore(points);
                    }
                    catch (Exception e)
                    {
                        logger.Error(e, "Failed to add points.");
                    }
                    //Console.WriteLine($"Added {points} points to player {player.Name}. New score: {player.Score}");
                    
                }
                else
                {
                    logger.Warn("Invalid input. Please enter a valid number for points.");
                    //Console.WriteLine("Invalid input. Please enter a valid number for points.");
                }
            }
            else
            {
                logger.Warn("Invalid input. Please enter a valid player ID.");
                //Console.WriteLine("Invalid input. Please enter a valid player ID.");
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
                if (playerRepo?.FindPlayer(id) == null)
                {
                    logger.Warn($"Player with id {id} was not found.");
                    return;
                }
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
                    //logger.Info($"Wallet {newWallet} added to player with id {id}");
                }
                else
                {
                    logger.Warn("Invalid input. Please enter a valid number for currency choice.");
                    //Console.WriteLine("Invalid input. Please enter a valid number for currency choice.");
                }
            }
            
        }
        private static List<IWallet>? ListWalletsForPlayer()
        {
            Console.WriteLine("Enter player id to list wallets:");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                if (playerRepo?.FindPlayer(id) == null)
                {
                    logger.Info("Player not found.");
                    //Console.WriteLine("Player not found.");
                    return null;
                }
                var wallets = walletRepo?.GetByPlayer(id);
                if (wallets == null || wallets.Count()==0) {
                    logger.Info("No wallet available.");
                    //Console.WriteLine("No wallets available.");
                    return null;
                }
                
                
                int i = 0;
                foreach (var wallet in wallets)
                {
                    Console.WriteLine($"{++i}. {wallet}");
                }
                return wallets;
                
            }
            else
            {
                logger.Warn("Invalid input. Please enter a valid player ID.");
                //Console.WriteLine("Invalid input. Please enter a valid player ID.");
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
            if (wallets == null)
            {
                return;
            }
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
                            logger.Error(e,"Failed to withdraw or deposit.");
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
                logger.Warn("Name cannot be empty. Please try again.");
                return;
            }
            playerRepo?.AddPlayer(new Player(name,playerRepo.GenerateId()));
        }
        private static void ListPlayers()
        {
            if (playerRepo == null)
            {
                logger.Error("Player repository is not initialized.");
                //Console.WriteLine("Player repository is not initialized.");
                return;
            }
            if (playerRepo?.GetAll().Count == 0)
            {
                logger.Info("No players found.");
                //Console.WriteLine("No players found.");
                return;
            }
            if (playerRepo?.GetAll() == null)
            {
                logger.Info("Player list is null.");
                //Console.WriteLine("Player list is null.");
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
                logger.Warn("Id cannot be empty. Please try again.");
                return;
            }
            var foundPlayer = playerRepo?.FindPlayer(id);
            if (foundPlayer != null)
            {
                Console.WriteLine($"Found: {foundPlayer}");

            }
            else
            {
                logger.Warn("Player not found.");
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
                logger.Warn("Invalid input. Please enter a valid player ID.");
                //Console.WriteLine("Invalid input. Please enter a valid player ID.");
            }
        }

        }
}
