using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaPay.Integrator.General.Application.Exceptions
{
    public class CustomerReferenceException : Exception
    {
        public CustomerReferenceException(string? message) : base(message)
        {
        }
    }
}
