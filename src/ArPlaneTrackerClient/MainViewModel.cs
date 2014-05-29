using FlightDataService.Models;
using GART.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;

namespace ArPlaneTrackerClient
{
    public class MainViewModel : NotifyBaseClass
    {
        private GeoCoordinate _currentLocation;
        private IFlightPositionClient _client;

        public MainViewModel()
        {
            ItemsToTrack = new ObservableCollection<ARItem>();
            _client = new FlightPositionClient();
            _client.NewData += _client_NewData;

            InfoText = "Starting";
        }

        private void _client_NewData(object sender, IList<FlightInfoDTO> e)
        {
            var arItems = new List<FlightInfoARItem>();

            foreach (var dto in e)
            {
                arItems.Add(dto.MapToARItem());
            }

            var handler = NewData;

            if (handler != null)
                handler(this, arItems);
        }

        public event EventHandler<IList<FlightInfoARItem>> NewData;

        public ObservableCollection<ARItem> ItemsToTrack { get; set; }

        public string InfoText
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public void SetPosition(GeoCoordinate geoCoordinate)
        {
            if (!geoCoordinate.IsUnknown)
            {
                if (_currentLocation != geoCoordinate)
                {
                    InfoText = "Position changed";

                    _currentLocation = geoCoordinate;

                    _client.SetLocation(_currentLocation.Latitude, _currentLocation.Longitude, _currentLocation.Altitude);
                }
            }
        }
    }
}