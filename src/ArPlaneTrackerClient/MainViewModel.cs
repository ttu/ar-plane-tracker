using GART.Data;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Threading.Tasks;

namespace ArPlaneTrackerClient
{
    public class MainViewModel : NotifyBaseClass
    {
        private GeoCoordinate _currentLocation;
        private IFlightPositionClient _client;

        public MainViewModel()
        {
            ItemsToTrack = new ObservableCollection<ARItem>();
            _client = new FlightPositionClientMock();
            InfoText = "Hello";
        }

        public ObservableCollection<ARItem> ItemsToTrack { get; set; }

        public string InfoText
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public async Task<ObservableCollection<ARItem>> GetPositionData(GeoCoordinate geoCoordinate)
        {
            if (!geoCoordinate.IsUnknown)
            {
                if (_currentLocation != geoCoordinate)
                {
                    _currentLocation = geoCoordinate;

                    var data = await _client.GetData();

                    var items = new ObservableCollection<ARItem>();

                    foreach (var flightDTO in data)
                    {
                        items.Add(flightDTO.MapToARItem());
                    }

                    //items.Add(new ARItem() { GeoLocation = new GeoCoordinate(22.20, 114.11), Content = "Hong Kong" });
                    //items.Add(new ARItem() { GeoLocation = new GeoCoordinate(59.17, 18.03), Content = "Stockholm" });
                    //items.Add(new ARItem() { GeoLocation = new GeoCoordinate(35.40, 139.45), Content = "Tokyo" });
                    //items.Add(new ARItem() { GeoLocation = new GeoCoordinate(47.30, 19.05), Content = "Budapest" });
                    //items.Add(new ARItem() { GeoLocation = new GeoCoordinate(40.42, 74), Content = "A" });
                    //items.Add(new ARItem() { GeoLocation = new GeoCoordinate(55.45, 37.36), Content = "Moscow" });
                    //items.Add(new ARItem() { GeoLocation = new GeoCoordinate(60.1, 25), Content = "Helsinki" });

                    return items;
                }
            }

            return null;
        }
    }
}