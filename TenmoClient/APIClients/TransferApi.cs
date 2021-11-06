using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using TenmoClient.Models;

namespace TenmoClient
{

    public class TransferApi
    {
        private readonly string baseURL = "https://localhost:44315/";
        private RestClient client = new RestClient();

        public TransferApi(string apiToken)
        {
            client.Authenticator = new JwtAuthenticator(apiToken);
        }
        /// <summary>
        /// Gets user balance from database
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public decimal GetBalance(int userId)
        {
            RestRequest request = new RestRequest(baseURL + $"balance");
            request.AddParameter("userId", userId);

            IRestResponse<Balance> response = client.Get<Balance>(request);

            if (!HandleError(response))
            {
                return 0M;
            }

            Balance newBalance = response.Data;
            return newBalance.AccountBalance;
        }
        /// <summary>
        /// Gets selected transfer details from database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="transferId"></param>
        /// <returns></returns>
        public Transfer GetTransferById(int userId, int transferId)
        {
            RestRequest request = new RestRequest(baseURL + $"transfer");
            request.AddParameter("transferId", transferId);
            request.AddParameter("userId", userId);

            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);

            if (!HandleError(response))
            {
                return null;
            }

            Transfer transfer = response.Data[0];

            return transfer;
        }
        /// <summary>
        /// Gets a list of transfers from database in which the user is the sender or recipient
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Transfer> GetTransfers(int userId)
        {
            RestRequest request = new RestRequest(baseURL + $"transfer");
            request.AddParameter("transferId", 0);
            request.AddParameter("userID", userId);

            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);

            if (!HandleError(response))
            {
                return new List<Transfer>();
            }

            List<Transfer> transfers = response.Data;
            return transfers;
        }
        /// <summary>
        /// Sends the details of a new transfer to the database
        /// If transfer is approved, updates the balances of both users
        /// </summary>
        /// <param name="transfer"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Transfer CreateTransfer(Transfer transfer, int userId)
        {
            RestRequest request = new RestRequest(baseURL + $"transfer/{userId}");
            request.AddJsonBody(transfer);

            IRestResponse<Transfer> response = client.Post<Transfer>(request);

            if (!HandleError(response))
            {
                return null;
            }
            transfer = response.Data;
            if (transfer.TransferStatus == 2001) //If transfer is approved, updates the balances of both users
            {
                RestRequest request2 = new RestRequest(baseURL + $"balance/{userId}");
                request2.AddJsonBody(transfer);

                IRestResponse<decimal> response2 = client.Put<decimal>(request2);

                if (!HandleError(response2))
                {
                    Console.WriteLine("There was a problem completing the transfer");
                    return null;
                }

                Console.WriteLine("Transfer Complete!");
                Console.WriteLine("Your new balance is: " + response2.Data.ToString("C"));
            }
            return transfer;
        }
        /// <summary>
        /// Updates the transfer status of selected transfer
        /// If approved, updates the balance of both users
        /// </summary>
        /// <param name="transfer"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateTransfer(Transfer transfer, int userId)
        {
            RestRequest request = new RestRequest(baseURL + $"transfer/{userId}");
            request.AddJsonBody(transfer);

            IRestResponse<Transfer> response = client.Put<Transfer>(request);

            if (!HandleError(response))
            {
                return false;
            }
            if (transfer.TransferStatus == 2001)
            {
                RestRequest request2 = new RestRequest(baseURL + $"balance/{userId}");
                request2.AddJsonBody(transfer);

                IRestResponse<decimal> responseBalance = client.Put<decimal>(request2);

                if (!HandleError(responseBalance))
                {
                    Console.WriteLine("There was a problem completing the transfer");
                    return false;
                }

                Console.WriteLine("Transfer Complete!");
                Console.WriteLine("Your new balance is: " + responseBalance.Data.ToString("C"));
            }
            return true;
        }
        /// <summary>
        /// Displays error message of returned status code and returns appropriate value
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private bool HandleError(IRestResponse response)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                Console.WriteLine("You don't have permission to view that account");
                return false;
            }
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("You must be logged in to view account information");
                return false;
            }
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine("Could not find account information");
                return false;
            }
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                Console.WriteLine("Could not complete transfer: " + response.ErrorMessage);
            }
            if (!response.IsSuccessful)
            {
                Console.WriteLine("There was a problem accessing your account information");
                return false;
            }
            return true;
        }
    }
}
