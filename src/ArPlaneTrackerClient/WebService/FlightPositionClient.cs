using FlightDataService.Models;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ArPlaneTrackerClient
{
    public class FlightPositionClient : IFlightPositionClient
    {
        private IHubProxy _proxy;
        private HubConnection _hub;

        public FlightPositionClient()
        {
            Start();
        }

        public event EventHandler<IList<FlightInfoDTO>> NewData;

        public async Task Start()
        {
            try
            {
                _hub = new HubConnection("http://www.contoso.com/");    
                _proxy = _hub.CreateHubProxy("FlightsHub");
                _proxy.On<List<FlightInfoDTO>>("newData", datas => HandleDatas(datas));

                await _hub.Start();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private void HandleDatas(List<FlightInfoDTO> datas)
        {
            var handler = NewData;

            if (handler != null)
                handler(this, datas);
        }

        public async Task<IList<FlightInfoDTO>> GetData()
        {
            await Task.Delay(100);

            // TODO: REST request

            return new List<FlightInfoDTO>();
        }

        public void SetLocation(double latitude, double longitude, double altitude)
        {
            if (_hub.State == ConnectionState.Connected)
                _proxy.Invoke("SetLocation", latitude, longitude, altitude);
        }
    }
}