using NovaPay.Integrator.Common.Data.DTOs;
using NovaPay.Integrator.Common.Services.Resources.Requests;

namespace NovaPay.Integrator.General.Application.Services
{
    public interface ILocalService
    {
        Task<bool> PostTransaction(string serviceUniqueId, TransactionRequest request);

        Task<bool> PostProductTransaction(string serviceUniqueId, ProductTransactionRequest request);
        Task<CustomerReferenceDto> ValidateReference(string customerReference, string serviceIdentifier);
    }
}