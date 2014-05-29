using GART.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArPlaneTrackerClient
{
    public class FlightInfoARItem : ARItem
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double AltitudeM { get; set; }

        public double SpeedKmh { get; set; }

        public string Registration { get; set; }

        public string Source { get; set; }

        public string Destination { get; set; }

        public string Model { get; set; }

        public double DistanceToUserKm { get; set; }

        public string DistanceText { get { return string.Format("Dist: {0}m", DistanceToUserKm); } }

        public string AltitudeText { get { return string.Format("Alt: {0}m", AltitudeM); } }

    }
}
