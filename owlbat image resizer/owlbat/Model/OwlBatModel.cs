namespace owlbat.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class OwlBatModel : DbContext
    {
        public OwlBatModel()
            : base("name=OwlBatEntities")
        {
        }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(e => e.AccountType)
                .IsFixedLength();
        }
    }
}
