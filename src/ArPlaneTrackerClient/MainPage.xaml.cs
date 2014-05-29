using GART.BaseControls;
using Microsoft.Devices;
using Microsoft.Devices.Sensors;
using Microsoft.Phone.Controls;
using System;
using System.Device.Location;
using System.Threading.Tasks;
using System.Windows;

namespace ArPlaneTrackerClient
{
    public partial class MainPage : PhoneApplicationPage
    {
        private GeoCoordinateWatcher _watcher;
        private MainViewModel _vm;

        // Constructor
        public MainPage()
        {
            // TODO: DI
            _vm = new MainViewModel();
            this.DataContext = _vm;

            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Start AR services
            ArDisplay.StartServices();

            if (!PhotoCamera.IsCameraTypeSupported(CameraType.Primary))
            {
                this.Dispatcher.BeginInvoke(() =>
                {
                    _vm.InfoText = "A Camera is not available on this phone.";
                });

                return;
            }

            if (!Motion.IsSupported)
            {
                this.Dispatcher.BeginInvoke(() =>
                {
                    _vm.InfoText = "The Motion API is not supported on this device.";
                });

                return;
            }

            if (_watcher == null)
            {
                _watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
                _watcher.PositionChanged += _watcher_PositionChanged;
            }

            try
            {
                _watcher.Start();
            }
            catch (Exception)
            {
                MessageBox.Show("unable to start the Motion API.");
            }
        }

        private async void _watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            // TODO: How to do binding to ARItems?
            var newData = await _vm.GetPositionData(e.Position.Location);

            if (newData != null)
                ArDisplay.ARItems = newData;
        }

        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            // Stop AR services
            ArDisplay.StopServices();

            if (_watcher != null)
            {
                _watcher.Dispose();
            }
        }

        // Ensure that the viewfinder is upright in LandscapeRight.
        protected override void OnOrientationChanged(OrientationChangedEventArgs e)
        {
            if (ArDisplay != null)
            {
                var orientation = ControlOrientation.Default;

                switch (e.Orientation)
                {
                    case PageOrientation.LandscapeLeft:
                        orientation = ControlOrientation.Clockwise270Degrees;
                        ArDisplay.Visibility = Visibility.Visible;
                        break;

                    case PageOrientation.LandscapeRight:
                        orientation = ControlOrientation.Clockwise90Degrees;
                        ArDisplay.Visibility = Visibility.Visible;
                        break;
                }

                ArDisplay.Orientation = orientation;
            }

            base.OnOrientationChanged(e);
        }
    }
}