using Newtonsoft.Json;
using NovaPay.Integrator.Common.Services.Resources.Requests;
using System.Text.Json.Serialization;

namespace NovaPay.Integrator.Geneeral.Api.Models.Requests
{
    public class PostTransactionRequest
    {
        public string? ServiceUniqueId { get; set; }
        public string? ServiceName { get; set; }
        public string? FinancialServiceUniqueId { get; set; }
        public string? FinancialServiceName { get; set; }

        public TransactionRequest TransactionRequest { get; set; }
    }
 
}
