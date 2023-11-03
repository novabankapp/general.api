using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaPay.Integrator.General.Application.Resources.Responses
{
    public class TransactionResponse
    {
        public string TransactionId { get; set; }
        public string Message { get; set; }
        public bool Successful { get; set; }
    }
}
