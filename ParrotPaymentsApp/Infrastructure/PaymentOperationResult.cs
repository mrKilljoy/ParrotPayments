using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParrotPaymentsApp.Infrastructure
{
    public class PaymentOperationResult
    {
        public bool Succeeded { get; set; }

        public Exception LastException { get; set; }
    }
}