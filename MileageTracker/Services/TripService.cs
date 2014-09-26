using MileageTracker.Extensions;
using MileageTracker.Infrastructure.DistanceCalculation;
using MileageTracker.Interfaces;
using MileageTracker.Models;
using MileageTracker.ViewModels;
using System;
using System.Data.Entity.Core;
using System.Linq;

namespace MileageTracker.Services {
    public class TripService : ITripService {
        private readonly IApplicationDbContext _dbContext;
        private readonly IDistanceCalculatorService _distanceCalculatorService;
        private readonly ApplicationUser _applicationUser;

        public TripService(IApplicationDbContext dbContext, ICurrentUserService currentUserService, IDistanceCalculatorService distanceCalculatorService) {
            _dbContext = dbContext;
            if (currentUserService != null && currentUserService.UserId != null) {
                _applicationUser = _dbContext.Users.SingleOrDefault(u => u.Id == currentUserService.UserId);
            }
            _distanceCalculatorService = distanceCalculatorService;
        }

        public PaginationViewModel GetTripsPaginated(int pageNumber, int pageSize) {
            return new PaginationViewModel {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalPages = (int) Math.Ceiling((double) _dbContext.Trips.Count(t => t.User.Id == _applicationUser.Id) / pageSize),
                Items = _dbContext.Trips.Where(t => t.User.Id == _applicationUser.Id)
                                        .OrderByDescending(t => t.Date)
                                        .ThenByDescending(t => t.Id)
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
            };
        }

        public Trip GetTripById(int id) {
            return _dbContext.Trips.SingleOrDefault(t => t.User.Id == _applicationUser.Id && t.Id == id);
        }

        public Trip GetNewTripTemplate() {
            var previousTrip = _dbContext.Trips.Where(t => t.User.Id == _applicationUser.Id).OrderByDescending(t => t.Date).FirstOrDefault();
            return new Trip {
                Date = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Utc),
                AddressOrigin = previousTrip == null ? null : previousTrip.AddressDestination,
                Car = previousTrip == null ? null : previousTrip.Car
            };
        }

        public Trip AddTrip(Trip trip) {
            trip.User = _applicationUser;

            trip.Date = trip.Date.ToUniversalTime();

            trip.AddressOrigin = trip.ResolveAddress(_dbContext, _applicationUser.Id, trip.AddressOrigin);
            trip.AddressDestination = trip.ResolveAddress(_dbContext, _applicationUser.Id, trip.AddressDestination);
            trip.Car = trip.ResolveCar(_dbContext, _applicationUser.Id, trip.Car);
            if (trip.AddressOrigin != null && trip.AddressDestination != null) {
                trip.DistanceInKm = _distanceCalculatorService.GetDistance(trip.AddressOrigin, trip.AddressDestination, Mode.Driving, Units.Metric);   
            }

            _dbContext.Trips.Add(trip);
            _dbContext.SaveChanges();

            return trip;
        }

        public Trip UpdateTrip(Trip trip) {
            var existingTrip = _dbContext.Trips.SingleOrDefault(t => t.User.Id == _applicationUser.Id && t.Id == trip.Id);
            if (existingTrip == null) {
                throw new ObjectNotFoundException();   
            }

            existingTrip.Date = trip.Date.ToUniversalTime();
            existingTrip.AddressOrigin = trip.ResolveAddress(_dbContext, _applicationUser.Id, trip.AddressOrigin);
            existingTrip.AddressDestination = trip.ResolveAddress(_dbContext, _applicationUser.Id, trip.AddressDestination);
            existingTrip.Car = trip.ResolveCar(_dbContext, _applicationUser.Id, trip.Car);
            existingTrip.Remarks = trip.Remarks;
            if (existingTrip.AddressOrigin != null && existingTrip.AddressDestination != null) {
                existingTrip.DistanceInKm = _distanceCalculatorService.GetDistance(existingTrip.AddressOrigin, existingTrip.AddressDestination, Mode.Driving, Units.Metric);
            }
            existingTrip.Remarks = trip.Remarks;

            _dbContext.SaveChanges();

            return existingTrip;
        }

        public bool DeleteTrip(int id) {
            var existingTrip = _dbContext.Trips.SingleOrDefault(t => t.User.Id == _applicationUser.Id && t.Id == id);
            if (existingTrip == null) {
                throw new ObjectNotFoundException();
            }
            _dbContext.Trips.Remove(existingTrip);
            _dbContext.SaveChanges();
            return true;
        }
    }
}