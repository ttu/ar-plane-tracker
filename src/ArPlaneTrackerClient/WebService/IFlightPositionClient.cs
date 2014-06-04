using FlightDataService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArPlaneTrackerClient
{
    public interface IFlightPositionClient
    {
        event EventHandler<IList<FlightInfoDTO>> NewData;

        bool IsConnected { get; }

        Task<bool> Start();

        void SetLocation(double latitude, double longitude, double altitude);

        Task<IList<FlightInfoDTO>> GetData();
    }
}