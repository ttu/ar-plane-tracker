using GART.Data;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Runtime.CompilerServices;

namespace ArPlaneTrackerClient
{
    public class FlightInfoARItem : ARItem
    {
        private readonly Dictionary<string, object> _propertyBackingDictionary = new Dictionary<string, object>();

        public double Latitude
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public double Longitude
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public double AltitudeM
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public double SpeedKmh
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        public string Registration
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string Source
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string Destination
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string Model
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public double DistanceToUserKm
        {
            get { return Get<double>(); }
            set { Set(value); }
        }

        private string Id { get { return this.Model + this.Registration + this.Source + this.Destination; } }

        public override bool Equals(object obj)
        {
            var item = obj as FlightInfoARItem;
            ;

            if (item == null)
            {
                return false;
            }

            return this.Id.Equals(item.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public void UpdateValuesFrom(FlightInfoARItem newItem)
        {
            this.Latitude = newItem.Latitude;
            this.Longitude = newItem.Longitude;
            this.SpeedKmh = newItem.SpeedKmh;
            this.AltitudeM = newItem.AltitudeM;
            this.DistanceToUserKm = newItem.DistanceToUserKm;
            this.GeoLocation = new GeoCoordinate(this.Latitude, this.Longitude, this.AltitudeM);
        }

        #region BaseClassStuff

        /// <summary>
        /// Return value for property from backing dictionary
        /// </summary>
        protected T Get<T>([CallerMemberName] string propertyName = null)
        {
            if (propertyName == null) throw new ArgumentNullException("propertyName");

            object value;
            if (_propertyBackingDictionary.TryGetValue(propertyName, out value))
            {
                return (T)value;
            }

            return default(T);
        }

        /// <summary>
        /// Set property value to backing dictionary
        /// </summary>
        protected bool Set<T>(T newValue, [CallerMemberName] string propertyName = null)
        {
            if (propertyName == null) throw new ArgumentNullException("propertyName");

            if (EqualityComparer<T>.Default.Equals(newValue, Get<T>(propertyName))) return false;

            _propertyBackingDictionary[propertyName] = newValue;
            NotifyPropertyChanged(propertyName);
            return true;
        }

        #endregion BaseClassStuff
    }
}