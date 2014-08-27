using System.Data.Entity.Validation;
using MileageTracker.Interfaces;
using MileageTracker.Models;
using MileageTracker.ViewModels;
using System;
using System.Data.Entity.Core;
using System.Linq;

namespace MileageTracker.Services {
    public class AddressService : IAddressService {
        private readonly IApplicationDbContext _dbContext;
        private readonly ApplicationUser _applicationUser;

        public AddressService(IApplicationDbContext dbContext, ICurrentUserService currentUserService) {
            _dbContext = dbContext;
            if (currentUserService != null && currentUserService.UserId != null) {
                _applicationUser = _dbContext.Users.SingleOrDefault(u => u.Id == currentUserService.UserId);
            }
        }

        public PaginationViewModel GetAddressesPaginated(int pageNumber, int pageSize) {
            return new PaginationViewModel {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)_dbContext.Addresses.Count(t => t.User.Id == _applicationUser.Id) / pageSize),
                Items = _dbContext.Addresses.Where(t => t.User.Id == _applicationUser.Id)
                                            .OrderBy(t => t.Name)
                                            .Skip((pageNumber - 1) * pageSize)
                                            .Take(pageSize)
            };
        }

        public Address GetAddressById(int id) {
            return _dbContext.Addresses.SingleOrDefault(t => t.User.Id == _applicationUser.Id && t.Id == id);
        }

        public Address AddAddress(Address address) {
            address.User = _applicationUser;

            _dbContext.Addresses.Add(address);
            _dbContext.SaveChanges();

            return address;
        }

        public Address UpdateAddress(Address address) {
            var existingAddress = _dbContext.Addresses.SingleOrDefault(t => t.User.Id == _applicationUser.Id && t.Id == address.Id);
            if (existingAddress == null) {
                throw new ObjectNotFoundException();
            }
            existingAddress.Name = address.Name;
            existingAddress.AddressLine = address.AddressLine;
            existingAddress.City = address.City;
            existingAddress.PostalCode = address.PostalCode;

            _dbContext.SaveChanges();

            return existingAddress;
        }

        public bool DeleteAddress(int id) {
            var address = _dbContext.Addresses.SingleOrDefault(t => t.User.Id == _applicationUser.Id && t.Id == id);
            if (address == null) {
                throw new ObjectNotFoundException();   
            }

            if (_dbContext.Trips.Any(t => t.AddressOrigin.Id == address.Id || t.AddressDestination.Id == address.Id)) {
                throw new DbEntityValidationException("Unable to delete. Address has been used in one or more trips.");
            }

            _dbContext.Addresses.Remove(address);
            _dbContext.SaveChanges();
            return true;
        }
    }
}