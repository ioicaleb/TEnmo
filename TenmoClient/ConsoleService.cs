using System;
using System.Collections.Generic;
using TenmoClient.Data;
using TenmoClient.Models;

namespace TenmoClient
{
    public class ConsoleService
    {
        /// <summary>
        /// Prompts for transfer ID to view, approve, or reject
        /// </summary>
        /// <param name="action">String to print in prompt. Expected values are "Approve" or "Reject" or "View"</param>
        /// <returns>ID of transfers to view, approve, or reject</returns>
        public int PromptForTransferID(string action)
        {
            Console.WriteLine("");
            Console.Write($"Please enter transfer ID to {action} (0 to cancel): ");

            if (!int.TryParse(Console.ReadLine(), out int auctionId))
            {
                Console.WriteLine("Invalid input. Only input a number.");
                return 0;
            }

            return auctionId;
        }

        public LoginUser PromptForLogin()
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();
            string password = GetPasswordFromConsole("Password: ");

            return new LoginUser
            {
                Username = username,
                Password = password
            };
        }

        private string GetPasswordFromConsole(string displayMessage)
        {
            string pass = "";
            Console.Write(displayMessage);
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                // Backspace Should Not Work
                if (!char.IsControl(key.KeyChar))
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Remove(pass.Length - 1);
                        Console.Write("\b \b");
                    }
                }
            }

            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter);
            Console.WriteLine("");

            return pass;
        }

        public int GetInteger(string message)
        {
            string userInput;
            int intValue;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
                if (userInput.ToLower() == "r")
                {
                    Console.WriteLine();
                    return -1;
                }
            }
            while (!int.TryParse(userInput, out intValue) || intValue < 1);
            Console.WriteLine();
            return intValue;
        }

        public decimal GetDecimal(string message)
        {
            string userInput;
            decimal decValue;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
                if (userInput.ToLower() == "r")
                {
                    Console.WriteLine();
                    return -1;
                }
            }
            while (!decimal.TryParse(userInput, out decValue) || decValue < 1);
            Console.WriteLine();
            return decValue;
        }

        public static bool GetBool(string message)
        {
            string userInput;
            bool boolValue;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
                if (userInput.ToLower() == "s" || userInput.ToLower() == "send")
                {
                    userInput = "true";
                }
                else if (userInput.ToLower() == "r" || userInput.ToLower() == "request")
                {
                    userInput = "false";
                }
            }
            while (!bool.TryParse(userInput, out boolValue));

            Console.WriteLine();
            return boolValue;
        }

        public Transfer GetTransferDetails(bool isSending)
        {
            Transfer transfer = new Transfer
            {
                OtherUserId = GetInteger("Which user ID do you want to transfer with?: "),
                Amount = GetDecimal("How much do you want in the transfer?: "),
            };

            if (isSending)
            {
                transfer.TransferType = 1001;
            }
            else 
            {
                transfer.TransferType = 1000;
            }
            return transfer;
        }
    }
}
