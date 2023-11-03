using NovaPay.Integrator.Common.Services.Resources.Requests;
using NovaPay.Integrator.General.Application.Resources.Responses;

namespace NovaPay.Integrator.General.Application.Services
{
    public interface IApiService
    {
        Task<AuthResponse> GetToken(string url, string grantType, string clientId, string clientSecret);

        Task<Common.Services.Resources.Responses.TransactionResponse> PostProductTransaction(string token, string url, ProductTransactionRequest request, bool isCustom = false, Dictionary<string, string>? fields = null);
        Task<Common.Services.Resources.Responses.TransactionResponse> PostTransaction(string token, string url, TransactionRequest request, bool isCustom = false, Dictionary<string, string>? fields = null);
        Task<Common.Services.Resources.Responses.ValidationResponse> ValidateReference(string token, string url, bool isCustom = false, Dictionary<string, string>? fields = null);
    }
}