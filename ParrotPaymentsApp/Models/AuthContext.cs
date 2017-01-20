using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ParrotPaymentsApp.Models
{
    public class AuthContext : IdentityDbContext<ParrotUser>
    {
        public AuthContext():base("AuthContext")
        {

        }

        public AuthContext(string conn_str) : base(conn_str) { }

        public virtual IDbSet<PaymentOperation> Payments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PaymentOperation>()
                    .HasRequired(m => m.Correspondent)
                    .WithMany(t => t.PaymentsReceived)
                    .HasForeignKey(m => m.CorrespondentId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<PaymentOperation>()
                    .HasRequired(m => m.Sender)
                    .WithMany(t => t.PaymentsSend)
                    .HasForeignKey(m => m.SenderId)
                    .WillCascadeOnDelete(false);
        }
    }
}