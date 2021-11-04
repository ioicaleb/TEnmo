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

        public decimal GetBalance(int userId)
        {
            RestRequest request = new RestRequest(baseURL + $"balance/{userId}");

            IRestResponse<Balance> response = client.Get<Balance>(request);

            if (!HandleError(response))
            {
                return 0M;
            }

            Balance newBalance = response.Data;
            return newBalance.AccountBalance;
        }

        public Transfer GetTransferById(int userId, int transferId)
        {
            RestRequest request = new RestRequest(baseURL + $"transfer/{userId}/");
            request.AddParameter("transferId", transferId);

            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);

            if (!HandleError(response))
            {
                return null;
            }

            Transfer transfer = response.Data[0];

            return transfer;
        }

        public List<Transfer> GetTransfers(int userId)
        {
            RestRequest request = new RestRequest(baseURL + $"transfer/{userId}");
            request.AddParameter("transferId", 0);

            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);

            if (!HandleError(response))
            {
                return new List<Transfer>();
            }

            List<Transfer> transfers = response.Data;
            return transfers;
        }

        public Transfer CreateTransfer(Transfer transfer, int userId)
        {
            RestRequest request = new RestRequest(baseURL + $"transfer/{userId}");
            request.AddJsonBody(transfer);

            IRestResponse response = client.Post(request);

            if (!HandleError(response))
            {
                return null;
            }
            if (transfer.TransferStatus == 2001)
            {
                request = new RestRequest(baseURL + $"balance/{userId}");
                request.AddJsonBody(transfer);

                response = client.Put(request);

                if (HandleError(response))
                {
                    Console.WriteLine("Transfer Complete!");
                }
            }
            return transfer;
        }

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
            if (!response.IsSuccessful)
            {
                Console.WriteLine("There was a problem accessing your account information");
                return false;
            }
            return true;
        }
    }
}
