using GART.BaseControls;
using GART.Data;
using Microsoft.Devices;
using Microsoft.Devices.Sensors;
using Microsoft.Phone.Controls;
using System;
using System.Collections.ObjectModel;
using System.Device.Location;
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
            _vm = new MainViewModel(this.Dispatcher);
            _vm.Start();

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
                SetLastItems();
            }
            catch (Exception)
            {
                _vm.InfoText = "unable to start the Motion API";
            }
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

        private void _watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            _vm.SetPosition(e.Position.Location);
        }

        private void SetLastItems()
        {
            if (!_vm.IsInitialized)
                return;

            _vm.WakeUp();

            this.Dispatcher.BeginInvoke(() =>
            {
                _vm.InfoText = string.Format("Data recovered: {0} planes", _vm.LastItems.Count);
                ArDisplay.ARItems = new ObservableCollection<ARItem>(_vm.LastItems);
            });
        }
    }
}