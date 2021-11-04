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
            RestRequest request = new RestRequest(baseURL + $"transfer/{userId}");
            request.AddParameter("transferId", transferId);

            IRestResponse<Transfer> response = client.Get<Transfer>(request);

            if (!HandleError(response))
            {
                return null;
            }
            return response.Data;
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
                Console.WriteLine("Could not find your account information");
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
