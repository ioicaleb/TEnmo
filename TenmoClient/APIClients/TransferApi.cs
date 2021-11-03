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
   
    public class TransferApi
    {
        private readonly string baseURL = "https://localhost:44315/";
        private RestClient client = new RestClient();

        public TransferApi(string apiToken)
        {
          client.Authenticator = new JwtAuthenticator(apiToken);
        }

        public TransferApi(RestClient client)
        {
            this.client = client;
        }
        
        public decimal GetBalance(int userId)
        {
            RestRequest request = new RestRequest(baseURL+ $"balance/{userId}");
            
            IRestResponse<Balance> response = client.Get<Balance>(request);
            return response.Data.AccountBalance;
            
        }
    }
}
