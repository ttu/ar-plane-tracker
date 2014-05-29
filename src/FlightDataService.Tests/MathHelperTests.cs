using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightDataService.Tests
{
    [TestClass]
    public class MathHelperTests
    {
        [TestMethod]
        public void Distance_SameSpot()
        {
            var a = new GeoPoint { Latitude = 60, Longitude = 20, ElevationInMeters = 0 };
            var b = new GeoPoint { Latitude = 60, Longitude = 20, ElevationInMeters = 100 };

            var dist = MathHelper.CalculateDistanceInKm(a, b);

            Assert.AreEqual(0.1, dist);
        }

        [TestMethod]
        public void Distance_Long()
        {
            var a = new GeoPoint { Latitude = 60, Longitude = 19, ElevationInMeters = 0 };
            var b = new GeoPoint { Latitude = 60, Longitude = 20, ElevationInMeters = 1000 };

            var dist = MathHelper.CalculateDistanceInKm(a, b);

            Assert.AreEqual(55.533, dist);
        }
    }
}