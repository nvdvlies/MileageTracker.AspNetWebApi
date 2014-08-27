using System.Data.Entity.Validation;
using MileageTracker.Interfaces;
using MileageTracker.Models;
using MileageTracker.ViewModels;
using System;
using System.Data.Entity.Core;
using System.Linq;

namespace MileageTracker.Services {
    public class CarService : ICarService {
        private readonly IApplicationDbContext _dbContext;
        private readonly ApplicationUser _applicationUser;

        public CarService(IApplicationDbContext dbContext, ICurrentUserService currentUserService) {
            _dbContext = dbContext;
            if (currentUserService != null && currentUserService.UserId != null) {
                _applicationUser = _dbContext.Users.SingleOrDefault(u => u.Id == currentUserService.UserId);
            }
        }

        public PaginationViewModel GetCarsPaginated(int pageNumber, int pageSize) {
            return new PaginationViewModel {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)_dbContext.Cars.Count(t => t.User.Id == _applicationUser.Id) / pageSize),
                Items = _dbContext.Cars.Where(c => c.User.Id == _applicationUser.Id)
                                        .OrderBy(c => c.Make)
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
            };
        }

        public Car GetCarById(int id) {
            return _dbContext.Cars.SingleOrDefault(t => t.User.Id == _applicationUser.Id && t.Id == id);
        }

        public Car AddCar(Car car) {
            car.User = _applicationUser;

            _dbContext.Cars.Add(car);
            _dbContext.SaveChanges();

            return car;
        }

        public Car UpdateCar(Car car) {
            var existingCar = _dbContext.Cars.SingleOrDefault(t => t.User.Id == _applicationUser.Id && t.Id == car.Id);
            if (existingCar == null) {
                throw new ObjectNotFoundException();
            }
            existingCar.Make = car.Make;
            existingCar.Model = car.Model;
            existingCar.NumberPlate = car.NumberPlate;
            existingCar.Remarks = car.Remarks;

            _dbContext.SaveChanges();

            return existingCar;
        }

        public bool DeleteCar(int id) {
            var car = _dbContext.Cars.SingleOrDefault(t => t.User.Id == _applicationUser.Id && t.Id == id);
            if (car == null) {
                throw new ObjectNotFoundException();   
            }

            if (_dbContext.Trips.Any(t => t.Car.Id == car.Id)) {
                throw new DbEntityValidationException("Unable to delete. Car has been used in one or more trips.");
            }

            _dbContext.Cars.Remove(car);
            _dbContext.SaveChanges();
            return true;
        }
    }
}