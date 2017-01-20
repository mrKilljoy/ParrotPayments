using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ParrotPaymentsApp.Models;

namespace ParrotPaymentsApp.Infrastructure
{
    public static class PaymentVMConverter
    {
        public static PaymentVM ConvertToViewModel(PaymentOperation payment)
        {
            return new PaymentVM
            {
                SenderName = payment.Sender.DecorativeName,
                CorrespondentName = payment.Correspondent.DecorativeName,
                Amount = payment.Amount,
                SenderPostBalance = payment.SenderPostBalance,
                CorrespondentPostBalance = payment.CorrespondentPostBalance,
                TransactionDate = payment.Date
            };
        }
    }
}