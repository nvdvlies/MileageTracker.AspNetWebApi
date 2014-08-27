using MileageTracker.Interfaces;
using MileageTracker.Models;
using MileageTracker.Repositories;
using System.Linq;

namespace MileageTracker.Extensions {
    public static class TripExtensions {
        public static Address ResolveAddress(this Trip trip, IApplicationDbContext dbContext, string userId, Address address) {
            if (address != null && address.Id > 0) {
                return dbContext.Addresses.SingleOrDefault(a => a.User.Id == userId && a.Id == address.Id);
            }
            return null;
        }

        public static Car ResolveCar(this Trip trip, IApplicationDbContext dbContext, string userId, Car car) {
            if (car != null && car.Id > 0) {
                return dbContext.Cars.SingleOrDefault(c => c.User.Id == userId && c.Id == car.Id);
            }
            return null;
        }

    }
}