using System;
using System.Collections.Generic;
using TenmoClient.Models;

namespace TenmoClient
{
    public class DisplayHelper
    {
        public void DisplayPendingTransferList(List<User> users, List<Transfer> transfers)
        {
            Console.WriteLine("".PadRight(55, '-'));
            Console.WriteLine("Transfers");
            Console.WriteLine(string.Format("{0,-15}{1,-30}{2,-10}{3}", "ID", "From/To", "Amount", "Status"));
            Console.WriteLine("".PadRight(55, '-'));
            foreach (Transfer transfer in transfers)
            {
                if (transfer.TransferStatus == 2000)
                {
                    foreach (User user in users)
                    {
                        if (transfer.TransferDirection == "From")
                        {
                            if (user.AccountId == transfer.AccountFrom)
                            {
                                transfer.OtherUser = user.Username;
                            }
                        }
                        else
                        {
                            if (user.AccountId == transfer.AccountTo)
                            {
                                transfer.OtherUser = user.Username;
                            }
                        }
                    }
                }
                Console.WriteLine(transfer);
            }
        }

        public void DisplayTransfer(List<User> users, Transfer transfer)
        {
            string toUser = null;
            string fromUser = null;
            foreach (User user in users)
            {
                switch (transfer.TransferDirection)
                {
                    case "From":
                        if (user.AccountId == transfer.AccountFrom)
                        {
                            fromUser = user.Username;
                            toUser = "Me";
                        }
                        break;
                    default:
                        if (user.AccountId == transfer.AccountTo)
                        {
                            toUser = user.Username;
                            fromUser = "Me";
                        }
                        break;
                }
            }
            Console.WriteLine("".PadRight(55, '-'));
            Console.WriteLine("Transfer Details");
            Console.WriteLine("".PadRight(55, '-'));
            Console.WriteLine(string.Format("{0,8}{6}\n{1,8}{7}\n{2,8}{8}\n{3,8}{9}\n{4,8}{10}\n{5,8}{11}",
                "ID: ", "From: ", "To: ", "Type: ", "Status: ", "Amount: ",
                transfer.TransferId, fromUser, toUser, transfer.TransferType, transfer.TransferStatus, transfer.Amount));
        }

        public void DisplayTransferList(List<User> users, List<Transfer> transfers)
        {
            Console.WriteLine("".PadRight(55, '-'));
            Console.WriteLine("Transfers");
            Console.WriteLine(string.Format("{0,-15}{1,-30}{2,-10}{3}", "ID", "From/To", "Amount", "Status"));
            Console.WriteLine("".PadRight(55, '-'));
            foreach (Transfer transfer in transfers)
            {
                foreach (User user in users)
                {
                    if (transfer.TransferDirection == "From")
                    {
                        if (user.AccountId == transfer.AccountFrom)
                        {
                            transfer.OtherUser = user.Username;
                        }
                    }
                    else
                    {
                        if (user.AccountId == transfer.AccountTo)
                        {
                            transfer.OtherUser = user.Username;
                        }
                    }
                }
                Console.WriteLine(transfer);
            }
        }

        public void DisplayUsersList(List<User> users)
        {
            Console.WriteLine("".PadRight(55, '-'));
            Console.WriteLine("Users");
            Console.WriteLine(string.Format("{0,-15}{1}", "ID", "Name"));
            Console.WriteLine("".PadRight(55, '-'));
            foreach (User user in users)
            {
                Console.WriteLine(string.Format("{0,-15}{1}", user.UserId, user.Username));
            }
        }
    }
}