using MileageTracker.Interfaces;
using MileageTracker.Models;
using System.Data.Entity;

namespace MileageTracker.Test.Unit.Helpers {
    public class FakeApplicationDbContext : IApplicationDbContext {
        public FakeApplicationDbContext() {
            Users = new FakeDbSet<ApplicationUser>();
            Trips = new FakeDbSet<Trip>();
            Cars = new FakeDbSet<Car>();
            Addresses = new FakeDbSet<Address>();
        }

        public IDbSet<ApplicationUser> Users { get; set; }
        public IDbSet<Trip> Trips { get; set; }
        public IDbSet<Car> Cars { get; set; }
        public IDbSet<Address> Addresses { get; set; }

        public int SaveChanges() {
            return 0;
        }

        public void Dispose() {
        }
    }
}
