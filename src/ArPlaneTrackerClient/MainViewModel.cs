using FlightDataService.Models;
using GART.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ArPlaneTrackerClient
{
    public class MainViewModel : NotifyBaseClass
    {
        private GeoCoordinate _currentLocation;
        private IFlightPositionClient _client;
        private Dispatcher _dispatcher;

        public MainViewModel(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;

            ItemsToTrack = new ObservableCollection<ARItem>();
            LastItems = new List<FlightInfoARItem>();

            _client = new FlightPositionClient();
            _client.NewData += _client_NewData;

            InfoText = "Starting";
        }

        public event EventHandler<IList<FlightInfoARItem>> NewData;

        public bool IsInitialized { get { return _currentLocation != null; } }

        public ObservableCollection<ARItem> ItemsToTrack { get; set; }

        public IList<FlightInfoARItem> LastItems { get; set; }

        public string InfoText
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public async Task Start()
        {
            var success = await _client.Start();

            if (success)
                InfoText = "Connected";
            else
                InfoText = "Connection failed";

            if (_currentLocation != null)
                SetPosition(_currentLocation);
        }

        public void SetPosition(GeoCoordinate geoCoordinate)
        {
            if (!geoCoordinate.IsUnknown)
            {
                if (_currentLocation != geoCoordinate)
                {
                    _currentLocation = geoCoordinate;

                    if (!_client.IsConnected)
                        return;

                    InfoText = "Position updated";

                    _client.SetLocation(_currentLocation.Latitude, _currentLocation.Longitude, _currentLocation.Altitude);
                }
            }
        }

        private void _client_NewData(object sender, IList<FlightInfoDTO> flightInfos)
        {
            var arItems = new List<FlightInfoARItem>();

            foreach (var dto in flightInfos)
            {
                arItems.Add(dto.MapToARItem());
            }

            LastItems = arItems;

            _dispatcher.BeginInvoke(() =>
            {
                InfoText = string.Format("{0} - Received {1} planes", DateTime.Now.ToLongTimeString(), arItems.Count);

                var toRemove = ItemsToTrack.Where(i => !arItems.Contains(i)).ToList();
                toRemove.ForEach(i => ItemsToTrack.Remove(i));

                var toUpdate = ItemsToTrack.Where(i => arItems.Contains(i)).ToList();
                toUpdate.ForEach(i => ((FlightInfoARItem)i).UpdateValuesFrom(arItems.Single(a => a.GetHashCode() == i.GetHashCode())));

                var toAdd = arItems.Where(i => !ItemsToTrack.Contains(i)).ToList();
                toAdd.ForEach(i => ItemsToTrack.Add(i));
            });

            //var handler = NewData;

            //if (handler != null)
            //    handler(this, arItems);
        }

        internal void WakeUp()
        {
            //throw new NotImplementedException();
        }
    }
}