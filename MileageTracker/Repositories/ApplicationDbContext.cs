using Microsoft.AspNet.Identity.EntityFramework;
using MileageTracker.Interfaces;
using MileageTracker.Models;
using System.Data.Entity;

namespace MileageTracker.Repositories {
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext {
        public ApplicationDbContext()
            : base("MileageTracker") {
        }

        public static ApplicationDbContext Create() {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUser>().ToTable("Users");
            modelBuilder.Entity<ApplicationUser>().ToTable("Users");

            modelBuilder.Entity<Trip>()
                .HasRequired(t => t.AddressOrigin)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Trip>()
                .HasRequired(t => t.AddressDestination)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Trip>()
                .HasRequired(t => t.Car)
                .WithMany()
                .WillCascadeOnDelete(false);
        }

        public IDbSet<Trip> Trips { get; set; }
        public IDbSet<Car> Cars { get; set; }
        public IDbSet<Address> Addresses { get; set; }
    }
}
