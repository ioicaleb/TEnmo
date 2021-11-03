using RestSharp;
using RestSharp.Authenticators;
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
            RestRequest request = new RestRequest(baseURL+ $"balance/{userId}");
            
            IRestResponse<Balance> response = client.Get<Balance>(request);
            return response.Data.AccountBalance;
            
        }
    }
}
