using Microsoft.VisualStudio.TestTools.UnitTesting;
using MileageTracker.Controllers;
using MileageTracker.Interfaces;
using MileageTracker.Models;
using MileageTracker.ViewModels;
using Moq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MileageTracker.Test.Unit.Controllers {
    [TestClass]
    public class CarsControllerTests {

        [TestMethod]
        public void GetCarsPaginated() {
            // Arrange
            const int pageNumber = 2;
            const int pageSize = 2;
            var mockCarService = new Mock<ICarService>();
            mockCarService.Setup(x => x.GetCarsPaginated(pageNumber, pageSize))
                .Returns(new PaginationViewModel {
                    CurrentPage = pageNumber,
                    PageSize = pageSize
                });

            var carController = new CarsController(mockCarService.Object) {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = carController.GetCarsPaginated(pageNumber, pageSize);

            // Assert
            PaginationViewModel paginationViewModel;
            Assert.IsTrue(response.TryGetContentValue(out paginationViewModel));
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void GetCarById() {
            // Arrange
            const int carId = 4;
            var mockCarService = new Mock<ICarService>();
            mockCarService.Setup(x => x.GetCarById(carId))
                .Returns(new Car {
                    Id = carId
                });
            var carController = new CarsController(mockCarService.Object) {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = carController.Get(carId);

            // Assert
            Car car;
            Assert.IsTrue(response.TryGetContentValue(out car));
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(carId, car.Id);
        }

        [TestMethod]
        public void GetCarById_NoMatch() {
            // Arrange
            const int carId = 15;
            var mockCarService = new Mock<ICarService>();
            mockCarService.Setup(x => x.GetCarById(carId))
                .Returns((Car)null);
            var carController = new CarsController(mockCarService.Object) {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = carController.Get(carId);

            // Assert
            Car car;
            Assert.IsFalse(response.TryGetContentValue(out car));
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void AddCar_CanAdd() {
            // Arrange
            var car = new Car {
                Id = 1
            };
            var mockCarService = new Mock<ICarService>();
            mockCarService.Setup(x => x.AddCar(car))
                .Returns(car);
            var carController = new CarsController(mockCarService.Object) {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = carController.AddCar(car);

            // Assert
            Car addedCar;
            Assert.IsTrue(response.TryGetContentValue(out addedCar));
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [TestMethod]
        public void UpdateCar_CanUpdate() {
            // Arrange
            var car = new Car {
                Id = 1
            };
            var mockCarService = new Mock<ICarService>();
            mockCarService.Setup(x => x.UpdateCar(car))
                .Returns(car);
            var carController = new CarsController(mockCarService.Object) {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = carController.UpdateCar(car.Id, car);

            // Assert
            Car addedCar;
            Assert.IsTrue(response.TryGetContentValue(out addedCar));
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void DeleteCar_CanDelete() {
            // Arrange
            const int carId = 4;
            var mockCarService = new Mock<ICarService>();
            mockCarService.Setup(x => x.DeleteCar(carId))
                .Returns(true);
            var carController = new CarsController(mockCarService.Object) {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = carController.DeleteCar(carId);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
