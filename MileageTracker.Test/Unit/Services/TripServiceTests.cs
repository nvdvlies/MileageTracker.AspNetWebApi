using System.Data.Entity.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MileageTracker.Infrastructure.DistanceCalculation;
using MileageTracker.Interfaces;
using MileageTracker.Models;
using MileageTracker.Services;
using MileageTracker.Test.Unit.Helpers;
using System;
using System.Linq;
using Moq;

namespace MileageTracker.Test.Unit.Services {
    [TestClass]
    public class TripServiceTests {
        private ApplicationUser _currentUser;
        private ApplicationUser _otherUser;
        private FakeApplicationDbContext _fakeApplicationDbContext;
        private Mock<ICurrentUserService> _mockCurrentUserService;
        private Mock<IDistanceCalculatorService> _mockIDistanceCalculatorService;

        [TestInitialize]
        public void BeforeEachTestmethod() {
            _currentUser = new ApplicationUser { Id = "12345" };
            _otherUser = new ApplicationUser { Id = "67890" };
            _fakeApplicationDbContext = new FakeApplicationDbContext();
            _fakeApplicationDbContext.Users.Add(_currentUser);
            _fakeApplicationDbContext.Users.Add(_otherUser);
            _mockCurrentUserService = new Mock<ICurrentUserService>();
            _mockCurrentUserService.Setup(x => x.UserId).Returns(_currentUser.Id);
            _mockIDistanceCalculatorService = new Mock<IDistanceCalculatorService>();
            _mockIDistanceCalculatorService.Setup(x => x.GetDistance(It.IsAny<Address>(), It.IsAny<Address>(), It.IsAny<Mode>(), It.IsAny<Units>()))
                .Returns(42);
        }

        [TestMethod]
        public void GetTripsPaginated() {
            // Arrange
            const int pageNumber = 2;
            const int pageSize = 2;
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 1, User = _currentUser });
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 2, User = _currentUser });
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 3, User = _currentUser });
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 4, User = _currentUser });
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 5, User = _currentUser });
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 6, User = _otherUser });
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 7, User = _otherUser });
            var tripService = new TripService(_fakeApplicationDbContext, _mockCurrentUserService.Object, _mockIDistanceCalculatorService.Object);

            // Act
            var paginationViewModel = tripService.GetTripsPaginated(pageNumber, pageSize);

            // Assert
            Assert.AreEqual(paginationViewModel.CurrentPage, pageNumber);
            Assert.AreEqual(paginationViewModel.PageSize, pageSize);
            Assert.AreEqual(paginationViewModel.TotalPages, 3); //user has 5 trips divided by a pagesize of 2 = 3 pages
            Assert.AreEqual(paginationViewModel.Items.Count(), pageSize);
            Assert.AreEqual(((Trip)paginationViewModel.Items.First()).Id, 3); //page 2 should contain third (and fourth) trip in case of a pagesize of 2
        }

        [TestMethod]
        public void GetTripById_Success() {
            // Arrange
            const int tripId = 4;
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 1, User = _currentUser });
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 2, User = _currentUser });
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 3, User = _currentUser });
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 4, User = _currentUser });
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 5, User = _currentUser });
            var tripService = new TripService(_fakeApplicationDbContext, _mockCurrentUserService.Object, _mockIDistanceCalculatorService.Object);

            // Act
            var trip = tripService.GetTripById(tripId);

            // Assert
            Assert.IsNotNull(trip);
            Assert.AreEqual(tripId, trip.Id);
        }

        [TestMethod]
        public void GetTripById_InvalidId() {
            // Arrange
            const int tripId = 8;
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 1, User = _currentUser });
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 2, User = _currentUser });
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 3, User = _currentUser });
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 4, User = _currentUser });
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 5, User = _currentUser });
            var tripService = new TripService(_fakeApplicationDbContext, _mockCurrentUserService.Object, _mockIDistanceCalculatorService.Object);

            // Act
            var trip = tripService.GetTripById(tripId);

            // Assert
            Assert.IsNull(trip);
        }

        [TestMethod]
        public void GetTripById_NotAllowed() {
            // Arrange
            const int tripId = 4;
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 1, User = _currentUser });
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 2, User = _currentUser });
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 3, User = _currentUser });
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 4, User = _otherUser });
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 5, User = _currentUser });
            var tripService = new TripService(_fakeApplicationDbContext, _mockCurrentUserService.Object, _mockIDistanceCalculatorService.Object);

            // Act
            var trip = tripService.GetTripById(tripId);

            // Assert
            Assert.IsNull(trip);
        }

        [TestMethod]
        public void GetNewTripTemplate_WithLatestUsedCarAndAddress() {
            // Arrange
            var latestUsedAddress = new Address { Id = 1 };
            var otherAddress = new Address { Id = 2 };
            var latestUsedCar = new Car { Id = 1 };
            var otherCar = new Car { Id = 1 };
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 1, User = _currentUser, Date = new DateTime(2014, 08, 01), AddressDestination = otherAddress, Car = otherCar });
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 2, User = _currentUser, Date = new DateTime(2014, 07, 01), AddressDestination = otherAddress, Car = otherCar });
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 3, User = _currentUser, Date = new DateTime(2014, 10, 01), AddressDestination = latestUsedAddress, Car = latestUsedCar });
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 4, User = _otherUser, Date = new DateTime(2014, 11, 01), AddressDestination = otherAddress, Car = otherCar });
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 5, User = _currentUser, Date = new DateTime(2014, 05, 01), AddressDestination = otherAddress, Car = otherCar });
            var tripService = new TripService(_fakeApplicationDbContext, _mockCurrentUserService.Object, _mockIDistanceCalculatorService.Object);

            // Act
            var trip = tripService.GetNewTripTemplate();

            // Assert
            Assert.IsNotNull(trip);
            Assert.IsNotNull(trip.AddressOrigin);
            Assert.IsNotNull(trip.Car);
            Assert.AreEqual(latestUsedAddress.Id, trip.AddressOrigin.Id);
            Assert.AreEqual(latestUsedCar.Id, trip.Car.Id);
        }

        [TestMethod]
        public void AddTrip_CanAdd() {
            // Arrange
            var addressOrigin = new Address { Id = 1, User = _currentUser};
            var addressDestination = new Address { Id = 2, User = _currentUser };
            var car = new Car { Id = 1, User = _currentUser };

            _fakeApplicationDbContext.Addresses.Add(addressOrigin);
            _fakeApplicationDbContext.Addresses.Add(addressDestination);
            _fakeApplicationDbContext.Cars.Add(car);

            var trip = new Trip {
                User = _currentUser,
                Date = new DateTime(2014, 08, 01),
                AddressOrigin = addressOrigin,
                AddressDestination = addressDestination,
                Car = car
            };
            var tripService = new TripService(_fakeApplicationDbContext, _mockCurrentUserService.Object, _mockIDistanceCalculatorService.Object);

            // Act
            trip = tripService.AddTrip(trip);

            // Assert
            Assert.IsNotNull(trip);
            Assert.IsNotNull(trip.AddressOrigin);
            Assert.IsNotNull(trip.AddressDestination);
            Assert.IsNotNull(trip.Car);
            Assert.AreEqual(addressOrigin.Id, trip.AddressOrigin.Id);
            Assert.AreEqual(addressDestination.Id, trip.AddressDestination.Id);
            Assert.AreEqual(car.Id, trip.Car.Id);
            Assert.AreEqual(trip.DistanceInKm, 42);
        }

        public void AddTrip_ShouldThrowValidationError() {
            // Arrange
            Exception caugthException = null;
            var trip = new Trip {
                User = _currentUser,
                Date = new DateTime(2014, 08, 01),
                AddressOrigin = null,
                AddressDestination = null,
                Car = null
            };
            var tripService = new TripService(_fakeApplicationDbContext, _mockCurrentUserService.Object, _mockIDistanceCalculatorService.Object);

            // Act
            try {
                trip = tripService.AddTrip(trip);
            } catch (DbEntityValidationException ex) {
                caugthException = ex;
            }
            
            // Assert
            Assert.IsNull(trip);
            Assert.IsNotNull(caugthException);
        }

        [TestMethod]
        public void UpdateTrip_CanUpdate() {
            // Arrange
            var addressOrigin = new Address { Id = 1, User = _currentUser };
            var addressDestination = new Address { Id = 2, User = _currentUser };
            var car = new Car { Id = 1, User = _currentUser };
            var otherCar = new Car { Id = 2, User = _currentUser };
            var otherAddress = new Address { Id = 3, User = _currentUser };
            var trip = new Trip {
                Id = 1,
                User = _currentUser,
                Date =  DateTime.SpecifyKind(new DateTime(2014, 08, 01), DateTimeKind.Utc),
                AddressOrigin = addressOrigin,
                AddressDestination = addressDestination,
                Car = car
            };

            _fakeApplicationDbContext.Addresses.Add(addressOrigin);
            _fakeApplicationDbContext.Addresses.Add(addressDestination);
            _fakeApplicationDbContext.Addresses.Add(otherAddress);
            _fakeApplicationDbContext.Cars.Add(car);
            _fakeApplicationDbContext.Cars.Add(otherCar);
            _fakeApplicationDbContext.Trips.Add(trip);

            var tripService = new TripService(_fakeApplicationDbContext, _mockCurrentUserService.Object, _mockIDistanceCalculatorService.Object);

            var updatedTrip = new Trip {
                Id = 1,
                User = _currentUser,
                Date = DateTime.SpecifyKind(new DateTime(2014, 08, 02), DateTimeKind.Utc),
                AddressOrigin = addressOrigin,
                AddressDestination = otherAddress,
                Car = otherCar,
                Remarks = ""
            };

            // Act
            updatedTrip = tripService.UpdateTrip(updatedTrip);

            // Assert
            Assert.IsNotNull(updatedTrip);
            Assert.IsNotNull(updatedTrip.AddressOrigin);
            Assert.IsNotNull(updatedTrip.AddressDestination);
            Assert.IsNotNull(updatedTrip.Car);
            Assert.AreEqual(new DateTime(2014, 08, 02), updatedTrip.Date);
            Assert.AreEqual(addressOrigin.Id, updatedTrip.AddressOrigin.Id);
            Assert.AreEqual(otherAddress.Id, updatedTrip.AddressDestination.Id);
            Assert.AreEqual(otherCar.Id, updatedTrip.Car.Id);
            Assert.AreEqual(updatedTrip.DistanceInKm, 42);
            Assert.AreEqual(updatedTrip.Id, 1);
        }


        [TestMethod]
        public void DeleteTrip_CanDelete() {
            const int tripId = 2;
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 1, User = _currentUser });
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 2, User = _currentUser });
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 3, User = _currentUser });
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 4, User = _otherUser });
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 5, User = _currentUser });
            var tripService = new TripService(_fakeApplicationDbContext, _mockCurrentUserService.Object, _mockIDistanceCalculatorService.Object);

            // Act
            var deleted = tripService.DeleteTrip(tripId);

            // Assert
            Assert.IsTrue(deleted);
        }
        
    }
}
