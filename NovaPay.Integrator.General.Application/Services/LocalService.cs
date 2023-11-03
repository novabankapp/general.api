using Common.Libraries.Services.Services;
using NovaPay.Integrator.Common.Data.DTOs;
using NovaPay.Integrator.Common.Data.Entities;
using NovaPay.Integrator.Common.Mapping;
using NovaPay.Integrator.Common.Services.Resources.Requests;
using NovaPay.Integrator.General.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaPay.Integrator.General.Application.Services
{
    public class LocalService : ILocalService
    {
        private readonly IService<CustomerReference, CustomerReferenceDto> _customerReferenceService;
        private readonly IService<ProcessedTransaction, ProcessedTransactionDto> _transactionService;

        public LocalService(IService<CustomerReference, CustomerReferenceDto> customerReferenceService,
            IService<ProcessedTransaction, ProcessedTransactionDto> transactionService)
        {
            _customerReferenceService = customerReferenceService;
            _transactionService = transactionService;
        }

        public async Task<CustomerReferenceDto> ValidateReference(string customerReference, string serviceIdentifier)
        {
            var response = await _customerReferenceService.GetOneAsync(c => c.ServiceUniqueIdentifier == serviceIdentifier
            && c.CustomerRef == customerReference);
            if (response == null)
            {
                throw new CustomerReferenceException("${customerReference} not found");
            }
            return MappingInstance.MainMapper.Map<CustomerReferenceDto>(response);
        }
        public async Task<bool> PostTransaction(string serviceUniqueId, TransactionRequest request)
        {
            var response = await _transactionService.CreateAsync(new ProcessedTransaction
            {
                Amount = request.Amount.Value,
                CustomerRef = request.CustomerRef,
                ServiceUniqueIdentifier = serviceUniqueId,
                TransactionDate = DateTime.Now,
                TransactionRef = request.TransactionRef,
            });

            return response != null;
        }

        public async Task<bool> PostProductTransaction(string serviceUniqueId, ProductTransactionRequest request)
        {
            var response = await _transactionService.CreateAsync(new ProcessedTransaction
            {
                Amount = request.Amount.Value,
                CustomerRef = request.ProductRef,
                ServiceUniqueIdentifier = serviceUniqueId,
                TransactionDate = DateTime.Now,
                TransactionRef = request.TransactionRef,
            });

            return response != null;
        }
    }
}
