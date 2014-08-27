using Microsoft.VisualStudio.TestTools.UnitTesting;
using MileageTracker.Controllers;
using MileageTracker.Interfaces;
using MileageTracker.Models;
using MileageTracker.Test.Unit.Helpers;
using MileageTracker.ViewModels;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Moq;

namespace MileageTracker.Test.Unit.Controllers {
    [TestClass]
    public class TripsControllerTests {

        [TestMethod]
        public void GetTripsPaginated() {
            // Arrange
            const int pageNumber = 2;
            const int pageSize = 2;
            var mockTripService = new Mock<ITripService>();
            mockTripService.Setup(x => x.GetTripsPaginated(pageNumber, pageSize))
                .Returns(new PaginationViewModel {
                    CurrentPage = pageNumber,
                    PageSize = pageSize
                });

            var tripController = new TripsController(mockTripService.Object) {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = tripController.GetTripsPaginated(pageNumber, pageSize);

            // Assert
            PaginationViewModel paginationViewModel;
            Assert.IsTrue(response.TryGetContentValue(out paginationViewModel));
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void GetTripById() {
            // Arrange
            const int tripId = 4;
            var mockTripService = new Mock<ITripService>();
            mockTripService.Setup(x => x.GetTripById(tripId))
                .Returns(new Trip {
                    Id = tripId
                });
            var tripController = new TripsController(mockTripService.Object) {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = tripController.Get(tripId);

            // Assert
            Trip trip;
            Assert.IsTrue(response.TryGetContentValue(out trip));
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(tripId, trip.Id);
        }

        [TestMethod]
        public void GetTripById_NoMatch() {
            // Arrange
            const int tripId = 15;
            var mockTripService = new Mock<ITripService>();
            mockTripService.Setup(x => x.GetTripById(tripId))
                .Returns((Trip)null);
            var tripController = new TripsController(mockTripService.Object) {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = tripController.Get(tripId);

            // Assert
            Trip trip;
            Assert.IsFalse(response.TryGetContentValue(out trip));
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void GetNewTripTemplate_WithLatestUsedCarAndAddress() {
            // Arrange
            var mockTripService = new Mock<ITripService>();
            mockTripService.Setup(x => x.GetNewTripTemplate())
                .Returns(new Trip {
                    Id = 1
                });
            var tripController = new TripsController(mockTripService.Object) {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = tripController.GetNewTripTemplate();

            // Assert
            Trip trip;
            Assert.IsTrue(response.TryGetContentValue(out trip));
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void AddTrip_CanAdd() {
            // Arrange
            var trip = new Trip {
                Id = 1
            };
            var mockTripService = new Mock<ITripService>();
            mockTripService.Setup(x => x.AddTrip(trip))
                .Returns(trip);
            var tripController = new TripsController(mockTripService.Object) {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = tripController.AddTrip(trip);

            // Assert
            Trip addedTrip;
            Assert.IsTrue(response.TryGetContentValue(out addedTrip));
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [TestMethod]
        public void UpdateTrip_CanUpdate() {
            // Arrange
            var trip = new Trip {
                Id = 1
            };
            var mockTripService = new Mock<ITripService>();
            mockTripService.Setup(x => x.UpdateTrip(trip))
                .Returns(trip);
            var tripController = new TripsController(mockTripService.Object) {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = tripController.UpdateTrip(trip.Id, trip);

            // Assert
            Trip addedTrip;
            Assert.IsTrue(response.TryGetContentValue(out addedTrip));
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void DeleteTrip_CanDelete() {
            // Arrange
            const int tripId = 4;
            var mockTripService = new Mock<ITripService>();
            mockTripService.Setup(x => x.DeleteTrip(tripId))
                .Returns(true);
            var tripController = new TripsController(mockTripService.Object) {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = tripController.DeleteTrip(tripId);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
