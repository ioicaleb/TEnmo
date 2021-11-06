using System;
using System.Collections.Generic;
using TenmoClient.Data;
using TenmoClient.Models;

namespace TenmoClient
{
    public class UserInterface
    {
        private readonly AuthService authService = new AuthService(); //Handles user login
        private readonly DisplayHelper display = new DisplayHelper(); //Handles the display of objects and menus
        private readonly RequestHandler requestHandler = new RequestHandler(); //Handles approval and rejection of pending requests
        private TransferApi transferApi;
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
            Console.WriteLine();
            Console.Write("Please choose an option: ");


            if (!int.TryParse(Console.ReadLine(), out int loginRegister))
            {
                Console.WriteLine();
                Console.WriteLine("Invalid input. Please enter only a number.");
                Console.WriteLine();
            }
            else if (loginRegister == 1)
            {
                Console.WriteLine();
                HandleUserLogin();

            }
            else if (loginRegister == 2)
            {
                Console.WriteLine();
                HandleUserRegister();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Invalid selection.");
                Console.WriteLine();
            }
        }

        private void ShowMainMenu()
        {
            List<User> users = authService.GetUsers();
            List<Transfer> transfers = new List<Transfer>();
            int menuSelection;
            do
            {
                Console.WriteLine("Welcome to TEnmo! Please make a selection: ");
                Console.WriteLine("1: View your current balance");
                Console.WriteLine("2: View all transfers");
                Console.WriteLine("3: Find transfer by ID");
                Console.WriteLine("4: View your pending requests"); //Also allows user to approve or reject requests
                Console.WriteLine("5: Send TE bucks");
                Console.WriteLine("6: Request TE bucks");
                Console.WriteLine("7: Log in as different user");
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
                            decimal balance = transferApi.GetBalance(UserService.UserId);
                            Console.WriteLine("Your Account Balance: " + balance.ToString("C"));
                            Console.WriteLine();
                            break;

                        case 2: // View All Transfers
                            transfers = transferApi.GetTransfers(UserService.UserId);
                            if (transfers.Count > 0)
                            {
                                display.DisplayTransferList(users, transfers);
                            }
                            Console.WriteLine();
                            break;

                        case 3: // View Specific Transfer
                            int transferId = ConsoleService.PromptForTransferID("search");
                            Transfer transfer = transferApi.GetTransferById(UserService.UserId, transferId);
                            if (!(transfer == null))
                            {
                                display.DisplayTransfer(users, transfer);
                            }
                            Console.WriteLine();
                            break;

                        case 4: // View Pending Requests
                            transfers = transferApi.GetTransfers(UserService.UserId);//Gets an updated list of transfers
                            List<Transfer> pendingTransfers = new List<Transfer>();
                            foreach (Transfer t in transfers) //Filters out any transfers that aren't pending
                            {
                                if (t.TransferStatus == 2000) //2000 = Pending
                                {
                                    pendingTransfers.Add(t);
                                }
                            }
                            if (pendingTransfers.Count > 0)
                            {
                                display.DisplayPendingTransferList(users, pendingTransfers); //Display transfer list
                                DisplayManageTransfersMenu(pendingTransfers); // Displays menu for approvals and rejections
                            }
                            else
                            {
                                Console.WriteLine("You have no pending transfers.");
                            }
                            Console.Clear();
                            break;

                        case 5: // Send TE Bucks
                            display.DisplayUsersList(users); //Displays Username and UserID of other users in the system
                            transfer = ConsoleService.GetTransferDetails(true); //Gets ID of other user in transfer and amount. True means current user is sending money
                            transfer = transferApi.CreateTransfer(transfer, UserService.UserId); //Sends transfer details to server to log
                            if (transfer != null) //If there is a problem with logging the details, transfer returns null
                            {
                                display.DisplayTransfer(users, transfer);
                            }
                            Console.WriteLine();
                            break;

                        case 6: // Request TE Bucks
                            display.DisplayUsersList(users);
                            transfer = ConsoleService.GetTransferDetails(false); //False means the current user is receiving money
                            transfer = transferApi.CreateTransfer(transfer, UserService.UserId);
                            display.DisplayTransfer(users, transfer);
                            Console.WriteLine();
                            break;

                        case 7: // Log in as someone else
                            Console.WriteLine();
                            UserService.ClearLoggedInUser(); //wipe out previous login info
                            Console.Clear();
                            return; // Leaves the menu and should return as someone else

                        case 0: // Quit
                            Console.Clear();
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
        /// <summary>
        /// Takes in user input to allow user to approve or reject pending transfers
        /// </summary>
        /// <param name="pendingTransfers"></param>
        private void DisplayManageTransfersMenu(List<Transfer> pendingTransfers)
        {
            bool leaveMenu = false;
            while (!leaveMenu)
            {
                int transferId = ConsoleService.PromptForTransferID("manage");
                if (transferId == 0)
                {
                    break;
                }
                Transfer selectedTransfer = new Transfer();
                foreach (Transfer t in pendingTransfers)
                {
                    if (t.TransferId == transferId)
                    {
                        selectedTransfer = t;
                    }
                }
                if (selectedTransfer.TransferId != 0)
                {
                    selectedTransfer = requestHandler.ManagePendingRequest(selectedTransfer); //Prompts user to approve or deny requests
                    transferApi.UpdateTransfer(selectedTransfer, UserService.UserId);
                }
                else
                {
                    Console.WriteLine("Invalid transfer ID");
                    Console.WriteLine();
                }

                leaveMenu = ConsoleService.GetBool("Are you finished managing transactions?(Y/N): ");
            }
        }
        /// <summary>
        /// Handles creating a new user
        /// </summary>
        private void HandleUserRegister()
        {
            bool isRegistered = false;

            while (!isRegistered) //will keep looping until user is registered
            {
                LoginUser registerUser = ConsoleService.PromptForLogin();
                isRegistered = authService.Register(registerUser);
            }

            Console.WriteLine();
            Console.WriteLine("Registration successful. You can now log in.");
            Console.WriteLine();
        }
        /// <summary>
        /// Handles logging in an existing user
        /// </summary>
        private void HandleUserLogin()
        {
            LoginUser loginUser = ConsoleService.PromptForLogin();

            if (authService.Login(loginUser))
            {
                transferApi = new TransferApi(UserService.Token);
                Console.Clear();
            }
            else
            {
                Console.WriteLine();
            }
        }
    }
}
