using NovaPay.Integrator.Common.Services.Resources.Requests;
using NovaPay.Integrator.Common.Services.Resources.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaPay.Integrator.General.Application.Services
{
    public class MockApiService : IApiService
    {
        public Task<Resources.Responses.AuthResponse> GetToken(string url, string grantType, string clientId, string clientSecret)
        {
            return Task.Run(() => new Resources.Responses.AuthResponse
            {
                 AccessToken = Guid.NewGuid().ToString("N"),
                 ExpiresIn = 3600,
                 RefreshToken = Guid.NewGuid().ToString("N"),
                 Scope = grantType,
                 TokenType = grantType,
            });
        }

        public Task<TransactionResponse> PostProductTransaction(string token, string url, ProductTransactionRequest request, bool isCustom = false, Dictionary<string, string>? fields = null)
        {
            return Task.Run(() => new TransactionResponse
            {

                Success = true,
                Payload = Guid.NewGuid().ToString("N"),

            });
        }

        public Task<TransactionResponse> PostTransaction(string token, string url, TransactionRequest request, bool isCustom = false, Dictionary<string, string>? fields = null)
        {
            return Task.Run(() => new TransactionResponse
            {
               
                 Success = true,
                 Payload = Guid.NewGuid().ToString("N"),

            });
        }

        public Task<ValidationResponse> ValidateReference(string token, string url, bool isCustom = false, Dictionary<string, string>? fields = null)
        {
            return Task.Run(() => new ValidationResponse
            {
                 Name = "Lewis Msasa",
                 Payload = new
                 {
                     id = "1234"
                 },
                 Success = true,

            });
        }
    }
}
