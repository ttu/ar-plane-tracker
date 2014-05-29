using FlightDataService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArPlaneTrackerClient
{
    public class FlightPositionClientMock : IFlightPositionClient
    {
        public event EventHandler<IList<FlightInfoDTO>> NewData;

        public void SetLocation(double latitude, double longitude, double altitude)
        {
            var datas = new List<FlightInfoDTO>();

            datas.Add(new FlightInfoDTO { Latitude = 22.20, Longitude = 114.11, AltitudeM = 1200, Model = "A330", DistanceToUserKm = 8 });
            datas.Add(new FlightInfoDTO { Latitude = 59.17, Longitude = 18.03, AltitudeM = 2000, Model = "A310", DistanceToUserKm = 12 });

            NewData(this, datas);
        }

        public async Task<IList<FlightInfoDTO>> GetData()
        {
            await Task.Delay(1000);

            var datas = new List<FlightInfoDTO>();

            datas.Add(new FlightInfoDTO { Latitude = 22.20, Longitude = 114.11, AltitudeM = 1200, Model = "A330", DistanceToUserKm = 8 });
            datas.Add(new FlightInfoDTO { Latitude = 59.17, Longitude = 18.03, AltitudeM = 2000, Model = "A310", DistanceToUserKm = 12 });

            return datas;
        }
    }
}