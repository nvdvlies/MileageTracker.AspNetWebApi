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
    public class CarServiceTests {
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
        public void GetCarsPaginated() {
            // Arrange
            const int pageNumber = 2;
            const int pageSize = 2;
            _fakeApplicationDbContext.Cars.Add(new Car { Id = 1, User = _currentUser });
            _fakeApplicationDbContext.Cars.Add(new Car { Id = 2, User = _currentUser });
            _fakeApplicationDbContext.Cars.Add(new Car { Id = 3, User = _currentUser });
            _fakeApplicationDbContext.Cars.Add(new Car { Id = 4, User = _currentUser });
            _fakeApplicationDbContext.Cars.Add(new Car { Id = 5, User = _currentUser });
            _fakeApplicationDbContext.Cars.Add(new Car { Id = 6, User = _otherUser });
            _fakeApplicationDbContext.Cars.Add(new Car { Id = 7, User = _otherUser });
            var carService = new CarService(_fakeApplicationDbContext, _mockCurrentUserService.Object);

            // Act
            var paginationViewModel = carService.GetCarsPaginated(pageNumber, pageSize);

            // Assert
            Assert.AreEqual(paginationViewModel.CurrentPage, pageNumber);
            Assert.AreEqual(paginationViewModel.PageSize, pageSize);
            Assert.AreEqual(paginationViewModel.TotalPages, 3); //user has 5 cars divided by a pagesize of 2 = 3 pages
            Assert.AreEqual(paginationViewModel.Items.Count(), pageSize);
            Assert.AreEqual(((Car)paginationViewModel.Items.First()).Id, 3); //page 2 should contain third (and fourth) car in case of a pagesize of 2
        }

        [TestMethod]
        public void GetCarById_Success() {
            // Arrange
            const int carId = 4;
            _fakeApplicationDbContext.Cars.Add(new Car { Id = 1, User = _currentUser });
            _fakeApplicationDbContext.Cars.Add(new Car { Id = 2, User = _currentUser });
            _fakeApplicationDbContext.Cars.Add(new Car { Id = 3, User = _currentUser });
            _fakeApplicationDbContext.Cars.Add(new Car { Id = 4, User = _currentUser });
            _fakeApplicationDbContext.Cars.Add(new Car { Id = 5, User = _currentUser });
            var carService = new CarService(_fakeApplicationDbContext, _mockCurrentUserService.Object);

            // Act
            var car = carService.GetCarById(carId);

            // Assert
            Assert.IsNotNull(car);
            Assert.AreEqual(carId, car.Id);
        }


        [TestMethod]
        public void GetCarById_InvalidId() {
            // Arrange
            const int carId = 8;
            _fakeApplicationDbContext.Cars.Add(new Car { Id = 1, User = _currentUser });
            _fakeApplicationDbContext.Cars.Add(new Car { Id = 2, User = _currentUser });
            _fakeApplicationDbContext.Cars.Add(new Car { Id = 3, User = _currentUser });
            _fakeApplicationDbContext.Cars.Add(new Car { Id = 4, User = _currentUser });
            _fakeApplicationDbContext.Cars.Add(new Car { Id = 5, User = _currentUser });
            var carService = new CarService(_fakeApplicationDbContext, _mockCurrentUserService.Object);

            // Act
            var car = carService.GetCarById(carId);

            // Assert
            Assert.IsNull(car);
        }

        [TestMethod]
        public void GetCarById_NotAllowed() {
            // Arrange
            const int carId = 4;
            _fakeApplicationDbContext.Cars.Add(new Car { Id = 1, User = _currentUser });
            _fakeApplicationDbContext.Cars.Add(new Car { Id = 2, User = _currentUser });
            _fakeApplicationDbContext.Cars.Add(new Car { Id = 3, User = _currentUser });
            _fakeApplicationDbContext.Cars.Add(new Car { Id = 4, User = _otherUser });
            _fakeApplicationDbContext.Cars.Add(new Car { Id = 5, User = _currentUser });
            var carService = new CarService(_fakeApplicationDbContext, _mockCurrentUserService.Object);

            // Act
            var car = carService.GetCarById(carId);

            // Assert
            Assert.IsNull(car);
        }

        [TestMethod]
        public void AddCar_CanAdd() {
            // Arrange
            var car = new Car {
                Make = "DeLorean",
                Model = "DMC-12",
                NumberPlate = "OUTATIME"
            };
            var carService = new CarService(_fakeApplicationDbContext, _mockCurrentUserService.Object);

            // Act
            car = carService.AddCar(car);

            // Assert
            Assert.IsNotNull(car);
            Assert.AreEqual(car.Make, "DeLorean");
            Assert.AreEqual(car.Model, "DMC-12");
            Assert.AreEqual(car.NumberPlate, "OUTATIME");
        }

        public void AddCar_ShouldThrowValidationError() {
            // Arrange
            Exception caugthException = null;
            var car = new Car {
                Make = "DeLorean",
                Model = "DMC-12",
                //NumberPlate = "OUTATIME"
            };
            var carService = new CarService(_fakeApplicationDbContext, _mockCurrentUserService.Object);

            // Act
            try {
                car = carService.AddCar(car);
            } catch (DbEntityValidationException ex) {
                caugthException = ex;
            }

            // Assert
            Assert.IsNull(car);
            Assert.IsNotNull(caugthException);
        }

        [TestMethod]
        public void UpdateCar_CanUpdate() {
            // Arrange
            var car = new Car {
                Id = 1,
                User = _currentUser,
                Make = "DCM-12",
                Model = "DeLorean",
                NumberPlate = "OUTOFTIME"
            };
            _fakeApplicationDbContext.Cars.Add(car);

            var carService = new CarService(_fakeApplicationDbContext, _mockCurrentUserService.Object);

            var updatedCar = new Car {
                Id = 1,
                Make = "DeLorean",
                Model = "DMC-12",
                NumberPlate = "OUTATIME"
            };

            // Act
            car = carService.UpdateCar(updatedCar);

            // Assert
            Assert.IsNotNull(updatedCar);
            Assert.AreEqual(car.Make, "DeLorean");
            Assert.AreEqual(car.Model, "DMC-12");
            Assert.AreEqual(car.NumberPlate, "OUTATIME");
        }


        [TestMethod]
        public void DeleteCar_CanDelete() {
            const int carId = 2;
            _fakeApplicationDbContext.Cars.Add(new Car { Id = 1, User = _currentUser });
            _fakeApplicationDbContext.Cars.Add(new Car { Id = 2, User = _currentUser });
            _fakeApplicationDbContext.Cars.Add(new Car { Id = 3, User = _currentUser });
            _fakeApplicationDbContext.Cars.Add(new Car { Id = 4, User = _otherUser });
            _fakeApplicationDbContext.Cars.Add(new Car { Id = 5, User = _currentUser });
            var carService = new CarService(_fakeApplicationDbContext, _mockCurrentUserService.Object);

            // Act
            var deleted = carService.DeleteCar(carId);

            // Assert
            Assert.IsTrue(deleted);
        }

        [TestMethod]
        public void DeleteCar_CantDeleteIfInUse() {
            Exception caugthException = null;
            var car = new Car {
                Id = 4,
                User = _currentUser
            };
            _fakeApplicationDbContext.Cars.Add(car);
            _fakeApplicationDbContext.Trips.Add(new Trip { Id = 1, User = _currentUser, Car = car });
            var carService = new CarService(_fakeApplicationDbContext, _mockCurrentUserService.Object);

            // Act
            bool deleted = false;
            try {
                deleted = carService.DeleteCar(car.Id);
            } catch (DbEntityValidationException ex) {
                caugthException = ex;
            }

            // Assert
            Assert.IsFalse(deleted);
            Assert.IsNotNull(caugthException);
        }

    }
}
