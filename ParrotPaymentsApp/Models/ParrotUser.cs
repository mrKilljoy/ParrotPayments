using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ParrotPaymentsApp.Models
{
    public class ParrotUser : IdentityUser
    {
        public int AccountBalance { get; set; } = 500;

        public string DecorativeName { get; set; }

        public virtual ICollection<PaymentOperation> PaymentsSend { get; set; }

        public virtual ICollection<PaymentOperation> PaymentsReceived { get; set; }
    }
}