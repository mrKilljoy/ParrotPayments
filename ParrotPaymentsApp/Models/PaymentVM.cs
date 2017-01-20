using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParrotPaymentsApp.Models
{
    public class PaymentVM
    {
        public string SenderName { get; set; }

        public DateTime TransactionDate { get; set; }

        public string CorrespondentName { get; set; }

        public int Amount { get; set; }

        public int SenderPostBalance { get; set; }

        public int CorrespondentPostBalance { get; set; }
    }
}