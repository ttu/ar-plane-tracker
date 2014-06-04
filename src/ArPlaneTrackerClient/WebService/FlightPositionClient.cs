using FlightDataService.Models;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArPlaneTrackerClient
{
    public class FlightPositionClient : IFlightPositionClient
    {
        private IHubProxy _proxy;
        private HubConnection _hub;

        public FlightPositionClient()
        {
            _hub = new HubConnection("http://ttuplanepositionservice.azurewebsites.net//");
            _proxy = _hub.CreateHubProxy("FlightsHub");
            _proxy.On<List<FlightInfoDTO>>("newData", datas => HandleDatas(datas));
        }

        public event EventHandler<IList<FlightInfoDTO>> NewData;

        public async Task<bool> Start()
        {
            await _hub.Start();

            if (_hub.State != ConnectionState.Connected)
                return false;

            return true;
        }

        public bool IsConnected { get { return _hub.State == ConnectionState.Connected; } }

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

        private void HandleDatas(List<FlightInfoDTO> datas)
        {
            var handler = NewData;

            if (handler != null)
                handler(this, datas);
        }
    }
}