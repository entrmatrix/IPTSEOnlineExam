using IPTSEOnlineExam.BLL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IPTSEOnlineExam.Models
{
    public partial class PaymentDataModel : DbContext
    {
        public PaymentDataModel()
            : base("name=LoginDataModel")
        {
        }

        public virtual DbSet<payment_details> payment_details { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<payment_details>()
                .Property(e => e.Id)
                .HasPrecision(18, 0);
        }
    }
}