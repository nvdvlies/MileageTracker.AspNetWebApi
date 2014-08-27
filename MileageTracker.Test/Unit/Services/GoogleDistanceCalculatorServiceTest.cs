using Microsoft.VisualStudio.TestTools.UnitTesting;
using MileageTracker.Infrastructure.DistanceCalculation;
using MileageTracker.Models;
using MileageTracker.Services;

namespace MileageTracker.Test.Unit.Services {
    [TestClass]
    public class GoogleDistanceCalculatorServiceTest {

        [TestMethod]
        public void GetDistance() {
            //Arrange
            var addressOrigin = new Address {
                Name = "Heineken",
                AddressLine = "Rietveldenweg 25",
                City = "'s-Hertogenbosch",
                PostalCode = "5222 AP"
            };
            var addressDestination = new Address {
                Name = "Grolsch",
                AddressLine = "Brouwerslaan 1",
                City = "Enschede",
                PostalCode = "7548 XA"
            };
            var distanceCalculator = new GoogleDistanceCalculatorService();

            //Act
            var distance = distanceCalculator.GetDistance(addressOrigin, addressDestination, Mode.Driving, Units.Metric);

            //Assert
            Assert.AreEqual(distance, 157);
        }
    }
}
