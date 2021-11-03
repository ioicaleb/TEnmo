using RestSharp;
using RestSharp.Authenticators;
using System;
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

            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                Console.WriteLine("You don't have permission to view that account");
                return 0M;
            }
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("You must be logged in to view account information");
                return 0M;
            }
            if (!response.IsSuccessful)
            {
                Console.WriteLine("There was a problem accessing your balnce information");
                return 0M;
            }

            Balance newBalance = response.Data;
            return newBalance.AccountBalance;
        }
    }
}
