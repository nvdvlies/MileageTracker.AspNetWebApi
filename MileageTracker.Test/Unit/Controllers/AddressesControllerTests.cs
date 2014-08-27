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
    public class AddressesControllerTests {

        [TestMethod]
        public void GetAddressesPaginated() {
            // Arrange
            const int pageNumber = 2;
            const int pageSize = 2;
            var mockAddressService = new Mock<IAddressService>();
            mockAddressService.Setup(x => x.GetAddressesPaginated(pageNumber, pageSize))
                .Returns(new PaginationViewModel {
                    CurrentPage = pageNumber,
                    PageSize = pageSize
                });

            var addressController = new AddressesController(mockAddressService.Object) {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = addressController.GetAddressesPaginated(pageNumber, pageSize);

            // Assert
            PaginationViewModel paginationViewModel;
            Assert.IsTrue(response.TryGetContentValue(out paginationViewModel));
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void GetAddressById() {
            // Arrange
            const int addressId = 4;
            var mockAddressService = new Mock<IAddressService>();
            mockAddressService.Setup(x => x.GetAddressById(addressId))
                .Returns(new Address {
                    Id = addressId
                });
            var addressController = new AddressesController(mockAddressService.Object) {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = addressController.Get(addressId);

            // Assert
            Address address;
            Assert.IsTrue(response.TryGetContentValue(out address));
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(addressId, address.Id);
        }

        [TestMethod]
        public void GetAddressById_NoMatch() {
            // Arrange
            const int addressId = 15;
            var mockAddressService = new Mock<IAddressService>();
            mockAddressService.Setup(x => x.GetAddressById(addressId))
                .Returns((Address)null);
            var addressController = new AddressesController(mockAddressService.Object) {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = addressController.Get(addressId);

            // Assert
            Address address;
            Assert.IsFalse(response.TryGetContentValue(out address));
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void AddAddress_CanAdd() {
            // Arrange
            var address = new Address {
                Id = 1
            };
            var mockAddressService = new Mock<IAddressService>();
            mockAddressService.Setup(x => x.AddAddress(address))
                .Returns(address);
            var addressController = new AddressesController(mockAddressService.Object) {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = addressController.AddAddress(address);

            // Assert
            Address addedAddress;
            Assert.IsTrue(response.TryGetContentValue(out addedAddress));
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [TestMethod]
        public void UpdateAddress_CanUpdate() {
            // Arrange
            var address = new Address {
                Id = 1
            };
            var mockAddressService = new Mock<IAddressService>();
            mockAddressService.Setup(x => x.UpdateAddress(address))
                .Returns(address);
            var addressController = new AddressesController(mockAddressService.Object) {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = addressController.UpdateAddress(address.Id, address);

            // Assert
            Address addedAddress;
            Assert.IsTrue(response.TryGetContentValue(out addedAddress));
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void DeleteAddress_CanDelete() {
            // Arrange
            const int addressId = 4;
            var mockAddressService = new Mock<IAddressService>();
            mockAddressService.Setup(x => x.DeleteAddress(addressId))
                .Returns(true);
            var addressController = new AddressesController(mockAddressService.Object) {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = addressController.DeleteAddress(addressId);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
