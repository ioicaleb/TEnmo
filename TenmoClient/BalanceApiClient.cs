using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Data;
using TenmoServer.DAO;
using RestSharp;
using TenmoClient.Models;
using RestSharp.Authenticators;
namespace TenmoClient
{
   
    public class BalanceApiClient
    {
        private readonly string baseURL = "https://localhost:44315/balance";
        private RestClient client = new RestClient();

        public BalanceApiClient(string apiToken)
        {
          client.Authenticator = new JwtAuthenticator(apiToken);
        }

        public BalanceApiClient(RestClient client)
        {
            this.client = client;
        }
        
        public decimal GetBalance(int userId)
        {
            RestRequest request = new RestRequest(baseURL+ $"/{userId}");
            
            IRestResponse<decimal> response = client.Get<decimal>(request);
            return response.Data;
            
        }
    }
}
