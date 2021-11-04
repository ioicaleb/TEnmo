﻿using System;
using System.Collections.Generic;
using TenmoClient.Data;
using TenmoClient.Models;

namespace TenmoClient
{
    public class UserInterface
    {
        private readonly ConsoleService consoleService = new ConsoleService();
        private readonly AuthService authService = new AuthService();
        private TransferApi balanceApi;
        private bool quitRequested = false;

        public void Start()
        {
            while (!quitRequested)
            {
                while (!UserService.IsLoggedIn)
                {
                    ShowLogInMenu();
                }

                // If we got here, then the user is logged in. Go ahead and show the main menu
                ShowMainMenu();
            }
        }

        private void ShowLogInMenu()
        {
            Console.WriteLine("Welcome to TEnmo!");
            Console.WriteLine("1: Login");
            Console.WriteLine("2: Register");
            Console.Write("Please choose an option: ");

            if (!int.TryParse(Console.ReadLine(), out int loginRegister))
            {
                Console.WriteLine("Invalid input. Please enter only a number.");
            }
            else if (loginRegister == 1)
            {
                HandleUserLogin();
            }
            else if (loginRegister == 2)
            {
                HandleUserRegister();
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }

        private void ShowMainMenu()
        {
            List<User> users = authService.GetUsers();
            int menuSelection;
            do
            {
                Console.WriteLine();
                Console.WriteLine("Welcome to TEnmo! Please make a selection: ");
                Console.WriteLine("1: View your current balance");
                Console.WriteLine("2: View your past transfers");
                Console.WriteLine("3: View your pending requests");
                Console.WriteLine("4: Send TE bucks");
                Console.WriteLine("5: Request TE bucks");
                Console.WriteLine("6: Log in as different user");
                Console.WriteLine("0: Exit");
                Console.WriteLine("---------");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out menuSelection))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else
                {
                    Console.WriteLine();
                    switch (menuSelection)
                    {
                        case 1: // View Balance
                            Console.WriteLine(balanceApi.GetBalance(UserService.UserId).ToString("C"));
                            break;

                        case 2: // View Past Transfers
                            int transferId = consoleService.PromptForTransferID("search");
                            List<Transfer> transfers = balanceApi.GetTransfers(UserService.UserId, transferId);
                            DisplayTransfers(users, transfers);
                            break;

                        case 3: // View Pending Requests
                            Console.WriteLine("NOT IMPLEMENTED!"); // TODO: Implement me
                            break;

                        case 4: // Send TE Bucks
                            Console.WriteLine("NOT IMPLEMENTED!"); // TODO: Implement me
                            break;

                        case 5: // Request TE Bucks
                            Console.WriteLine("NOT IMPLEMENTED!"); // TODO: Implement me
                            break;

                        case 6: // Log in as someone else
                            Console.WriteLine();
                            UserService.ClearLoggedInUser(); //wipe out previous login info
                            return; // Leaves the menu and should return as someone else

                        case 0: // Quit
                            Console.WriteLine("Goodbye!");
                            quitRequested = true;
                            return;

                        default:
                            Console.WriteLine("That doesn't seem like a valid choice.");
                            break;
                    }
                }
            } while (menuSelection != 0);
        }

        private static void DisplayTransfers(List<User> users, List<Transfer> transfers)
        {
            if (transfers.Count > 0)
            {
                Console.WriteLine(string.Format("{0,-15}{1,-30}{2}", "Transfer ID", "From/To", "Amount"));
                Console.WriteLine("".PadRight(55, '-'));
                foreach (Transfer transfer in transfers)
                {
                    foreach (User user in users)
                    {
                        if (transfer.TransferType == "From")
                        {
                            if (user.AccountId == transfer.AccountFromId)
                            {
                                transfer.OtherUser = user.Username;
                            }
                        }
                        else
                            if (user.AccountId == transfer.AccountToId)
                        {
                            transfer.OtherUser = user.Username;
                        }
                    }
                    Console.WriteLine(transfer);
                }
            }
            else
            {
                Console.WriteLine("Could not find your transfers");
            }
        }

        private void HandleUserRegister()
        {
            bool isRegistered = false;

            while (!isRegistered) //will keep looping until user is registered
            {
                LoginUser registerUser = consoleService.PromptForLogin();
                isRegistered = authService.Register(registerUser);
            }

            Console.WriteLine("");
            Console.WriteLine("Registration successful. You can now log in.");
        }

        private void HandleUserLogin()
        {
            while (!UserService.IsLoggedIn) //will keep looping until user is logged in
            {
                LoginUser loginUser = consoleService.PromptForLogin();

                if (authService.Login(loginUser))
                {
                    balanceApi = new TransferApi(UserService.Token);
                }
            }
        }
    }
}
