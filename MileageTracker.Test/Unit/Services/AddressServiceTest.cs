using Microsoft.VisualStudio.TestTools.UnitTesting;
using MileageTracker.Interfaces;
using MileageTracker.Models;
using MileageTracker.Services;
using MileageTracker.Test.Unit.Helpers;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using Moq;

namespace MileageTracker.Test.Unit.Services {
    [TestClass]
    public class AddressServiceTest {
        private ApplicationUser _currentUser;
        private ApplicationUser _otherUser;
        private FakeApplicationDbContext _fakeApplicationDbContext;
        private Mock<ICurrentUserService> _mockCurrentUserService;

        [TestInitialize]
        public void BeforeEachTestmethod() {
            _currentUser = new ApplicationUser { Id = "12345" };
            _otherUser = new ApplicationUser { Id = "67890" };
            _fakeApplicationDbContext = new FakeApplicationDbContext();
            _fakeApplicationDbContext.Users.Add(_currentUser);
            _fakeApplicationDbContext.Users.Add(_otherUser);
            _mockCurrentUserService = new Mock<ICurrentUserService>();
            _mockCurrentUserService.Setup(x => x.UserId).Returns(_currentUser.Id);
        }

        [TestMethod]
        public void GetAddressesPaginated() {
            // Arrange
            const int pageNumber = 2;
            const int pageSize = 2;
            _fakeApplicationDbContext.Addresses.Add(new Address { Id = 1, User = _currentUser });
            _fakeApplicationDbContext.Addresses.Add(new Address { Id = 2, User = _currentUser });
            _fakeApplicationDbContext.Addresses.Add(new Address { Id = 3, User = _currentUser });
            _fakeApplicationDbContext.Addresses.Add(new Address { Id = 4, User = _currentUser });
            _fakeApplicationDbContext.Addresses.Add(new Address { Id = 5, User = _currentUser });
            _fakeApplicationDbContext.Addresses.Add(new Address { Id = 6, User = _otherUser });
            _fakeApplicationDbContext.Addresses.Add(new Address { Id = 7, User = _otherUser });
            var addressService = new AddressService(_fakeApplicationDbContext, _mockCurrentUserService.Object);

            // Act
            var paginationViewModel = addressService.GetAddressesPaginated(pageNumber, pageSize);

            // Assert
            Assert.AreEqual(paginationViewModel.CurrentPage, pageNumber);
            Assert.AreEqual(paginationViewModel.PageSize, pageSize);
            Assert.AreEqual(paginationViewModel.TotalPages, 3); //user has 5 addresss divided by a pagesize of 2 = 3 pages
            Assert.AreEqual(paginationViewModel.Items.Count(), pageSize);
            Assert.AreEqual(((Address)paginationViewModel.Items.First()).Id, 3); //page 2 should contain third (and fourth) address in case of a pagesize of 2
        }

        [TestMethod]
        public void GetAddressById_Success() {
            // Arrange
            const int addressId = 4;
            _fakeApplicationDbContext.Addresses.Add(new Address { Id = 1, User = _currentUser });
            _fakeApplicationDbContext.Addresses.Add(new Address { Id = 2, User = _currentUser });
            _fakeApplicationDbContext.Addresses.Add(new Address { Id = 3, User = _currentUser });
            _fakeApplicationDbContext.Addresses.Add(new Address { Id = 4, User = _currentUser });
            _fakeApplicationDbContext.Addresses.Add(new Address { Id = 5, User = _currentUser });
            var addressService = new AddressService(_fakeApplicationDbContext, _mockCurrentUserService.Object);

            // Act
            var address = addressService.GetAddressById(addressId);

            // Assert
            Assert.IsNotNull(address);
            Assert.AreEqual(addressId, address.Id);
        }


        [TestMethod]
        public void GetAddressById_InvalidId() {
            // Arrange
            const int addressId = 8;
            _fakeApplicationDbContext.Addresses.Add(new Address { Id = 1, User = _currentUser });
            _fakeApplicationDbContext.Addresses.Add(new Address { Id = 2, User = _currentUser });
            _fakeApplicationDbContext.Addresses.Add(new Address { Id = 3, User = _currentUser });
            _fakeApplicationDbContext.Addresses.Add(new Address { Id = 4, User = _currentUser });
            _fakeApplicationDbContext.Addresses.Add(new Address { Id = 5, User = _currentUser });
            var addressService = new AddressService(_fakeApplicationDbContext, _mockCurrentUserService.Object);

            // Act
            var address = addressService.GetAddressById(addressId);

            // Assert
            Assert.IsNull(address);
        }

        [TestMethod]
        public void GetAddressById_NotAllowed() {
            // Arrange
            const int addressId = 4;
            _fakeApplicationDbContext.Addresses.Add(new Address { Id = 1, User = _currentUser });
            _fakeApplicationDbContext.Addresses.Add(new Address { Id = 2, User = _currentUser });
            _fakeApplicationDbContext.Addresses.Add(new Address { Id = 3, User = _currentUser });
            _fakeApplicationDbContext.Addresses.Add(new Address { Id = 4, User = _otherUser });
            _fakeApplicationDbContext.Addresses.Add(new Address { Id = 5, User = _currentUser });
            var addressService = new AddressService(_fakeApplicationDbContext, _mockCurrentUserService.Object);

            // Act
            var address = addressService.GetAddressById(addressId);

            // Assert
            Assert.IsNull(address);
        }

        [TestMethod]
        public void AddAddress_CanAdd() {
            // Arrange
            var address = new Address {
                Name = "Heineken",
                AddressLine = "Rietveldenweg 25",
                City = "'s-Hertogenbosch",
                PostalCode = "5222 AP"
            };
            var addressService = new AddressService(_fakeApplicationDbContext, _mockCurrentUserService.Object);

            // Act
            address = addressService.AddAddress(address);

            // Assert
            Assert.IsNotNull(address);
            Assert.AreEqual(address.Name, "Heineken");
            Assert.AreEqual(address.AddressLine, "Rietveldenweg 25");
            Assert.AreEqual(address.City, "'s-Hertogenbosch");
            Assert.AreEqual(address.PostalCode, "5222 AP");
        }

        public void AddAddress_ShouldThrowValidationError() {
            // Arrange
            Exception caugthException = null;
            var address = new Address {
                Name = "Heineken",
                AddressLine = "Rietveldenweg 25",
                City = "'s-Hertogenbosch",
                PostalCode = null
            };
            var addressService = new AddressService(_fakeApplicationDbContext, _mockCurrentUserService.Object);

            // Act
            try {
                address = addressService.AddAddress(address);
            } catch (DbEntityValidationException ex) {
                caugthException = ex;
            }

            // Assert
            Assert.IsNull(address);
            Assert.IsNotNull(caugthException);
        }

        [TestMethod]
        public void UpdateAddress_CanUpdate() {
            // Arrange
            var address = new Address {
                Id = 1,
                User = _currentUser,
                Name = "Grolsch",
                AddressLine = "Brouwerslaan 1",
                City = "Enschede",
                PostalCode = "7548 XA"
            };
            _fakeApplicationDbContext.Addresses.Add(address);

            var addressService = new AddressService(_fakeApplicationDbContext, _mockCurrentUserService.Object);

            var updatedAddress = new Address {
                Id = 1,
                Name = "Heineken",
                AddressLine = "Rietveldenweg 25",
                City = "'s-Hertogenbosch",
                PostalCode = "5222 AP"
            };

            // Act
            address = addressService.UpdateAddress(updatedAddress);

            // Assert
            Assert.IsNotNull(updatedAddress);
            Assert.AreEqual(address.Name, "Heineken");
            Assert.AreEqual(address.AddressLine, "Rietveldenweg 25");
            Assert.AreEqual(address.City, "'s-Hertogenbosch");
            Assert.AreEqual(address.PostalCode, "5222 AP");
        }


        [TestMethod]
        public void DeleteAddress_CanDelete() {
            const int addressId = 2;
            _fakeApplicationDbContext.Addresses.Add(new Address { Id = 1, User = _currentUser });
            _fakeApplicationDbContext.Addresses.Add(new Address { Id = 2, User = _currentUser });
            _fakeApplicationDbContext.Addresses.Add(new Address { Id = 3, User = _currentUser });
            _fakeApplicationDbContext.Addresses.Add(new Address { Id = 4, User = _otherUser });
            _fakeApplicationDbContext.Addresses.Add(new Address { Id = 5, User = _currentUser });
            var addressService = new AddressService(_fakeApplicationDbContext, _mockCurrentUserService.Object);

            // Act
            var deleted = addressService.DeleteAddress(addressId);

            // Assert
            Assert.IsTrue(deleted);
        }

        [TestMethod]
        public void DeleteAddress_CantDeleteIfInUse() {
            Exception caugthException = null;
            var address = new Address {
                Id = 4,
                User = _currentUser
            };
            _fakeApplicationDbContext.Addresses.Add(address);
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 1, User = _currentUser, AddressOrigin = address });
            var addressService = new AddressService(_fakeApplicationDbContext, _mockCurrentUserService.Object);

            // Act
            var deleted = false;
            try {
                deleted = addressService.DeleteAddress(address.Id);
            } catch (DbEntityValidationException ex) {
                caugthException = ex;
            }

            // Assert
            Assert.IsFalse(deleted);
            Assert.IsNotNull(caugthException);
        }
    }
}
