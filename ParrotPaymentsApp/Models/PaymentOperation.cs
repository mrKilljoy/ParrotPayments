using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParrotPaymentsApp.Models
{
    public class PaymentOperation
    {
        [Key]
        public long Id { get; set; }

        public DateTime Date { get; set; }
        
        [Required]
        public string SenderUsername { get; set; }

        [Required]
        public string CorrespondentUsername { get; set; }

        [Required]
        public int Amount { get; set; }

        public int SenderPostBalance { get; set; }

        public int CorrespondentPostBalance { get; set; }

        [ForeignKey("Sender")]
        public string SenderId { get; set; }

        [ForeignKey("Correspondent")]
        public string CorrespondentId { get; set; }
        
        public virtual ParrotUser Sender { get; set; }

        public virtual ParrotUser Correspondent { get; set; }
    }
}