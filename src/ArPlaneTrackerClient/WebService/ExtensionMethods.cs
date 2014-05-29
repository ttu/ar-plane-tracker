using FlightDataService.Models;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArPlaneTrackerClient
{
    public static class ExtensionMethods
    {
        public static FlightInfoARItem MapToARItem(this FlightInfoDTO info)
        {
            return new FlightInfoARItem { 
                AltitudeM = info.AltitudeM,
                Destination = info.Destination,
                DistanceToUserKm = info.DistanceToUserKm,
                Latitude = info.Latitude,
                Longitude = info.Longitude,
                Model = info.Model,
                Registration = info.Registration,
                Source = info.Source,
                SpeedKmh = info.SpeedKmh,
                GeoLocation = new GeoCoordinate(info.Latitude, info.Longitude, info.AltitudeM)
            };
        }
    }
}
