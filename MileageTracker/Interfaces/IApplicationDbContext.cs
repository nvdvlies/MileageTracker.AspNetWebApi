using MileageTracker.Models;
using System.Data.Entity;

namespace MileageTracker.Interfaces {
    public interface IApplicationDbContext {
        IDbSet<ApplicationUser> Users { get; set; }
        IDbSet<Address> Addresses { get; set; }
        IDbSet<Car> Cars { get; set; }
        IDbSet<Trip> Trips { get; set; }

        int SaveChanges();

        void Dispose();
    }
}
