using System.Data.Entity;
using AccountManager.MVVM.Models;

namespace AccountManager.Core
{
    public partial class Data : DbContext
    {
        public Data()
            : base("name=defaultConnection")
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(e => e.Groups)
                .WithOptional(e => e.User)
                .WillCascadeOnDelete();
        }
    }
}
