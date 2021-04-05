using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace GpsNote.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomMap : ContentView
    {
        #region -- Private static --

        private static Position _lastPosition;
        private static double _lastZoom;
        private static double _lastBearing;
        private static double _lastTilt;

        #endregion

        #region -- Publics static -- 

        public static readonly BindableProperty MapTypeProperty =
            BindableProperty.Create(nameof(MapType),
                                    typeof(MapType),
                                    typeof(CustomMap),
                                    defaultValue: MapType.Street,
                                    propertyChanged: MapTypePropertyChanged);

        public static readonly BindableProperty CurrentPositionProperty =
            BindableProperty.Create(nameof(CurrentPosition),
                                    typeof(Position),
                                    typeof(CustomMap),
                                    defaultValue: new Position(),
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: CurrentPositionPropertyChanged);


        public static readonly BindableProperty CurrentZoomProperty =
            BindableProperty.Create(nameof(CurrentZoom),
                                    typeof(double),
                                    typeof(CustomMap),
                                    defaultValue: 2.0,
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: CurrentZoomPropertyChanged);


        public static readonly BindableProperty IsRequiredCameraChangingProperty =
            BindableProperty.Create(nameof(IsRequiredCameraChanging),
                                    typeof(bool),
                                    typeof(CustomMap),
                                    defaultValue: false,
                                    propertyChanged: IsRequiredCameraChangingChanged);

        public static readonly BindableProperty CurrentBearingProperty =
            BindableProperty.Create(nameof(CurrentBearing),
                                    typeof(double),
                                    typeof(CustomMap),
                                    defaultValue: default(double),
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: CurrentBearingPropertyChanged);



        public static readonly BindableProperty CurrentTiltProperty =
            BindableProperty.Create(nameof(CurrentTilt),
                                    typeof(double),
                                    typeof(CustomMap),
                                    defaultValue: default(double),
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: CurrentTiltPropertyChanged);


        public static readonly BindableProperty PinsProperty =
            BindableProperty.Create(nameof(Pins),
                                    typeof(List<Pin>),
                                    typeof(CustomMap),
                                    propertyChanged: PinsPropertyChanged);


        public static readonly BindableProperty PinAddedProperty =
            BindableProperty.Create(nameof(PinAdded),
                                    typeof(Pin),
                                    typeof(CustomMap));




        #endregion


        public CustomMap()
        {
            InitializeComponent();
        }


        #region -- Publics -- 

        public MapType MapType
        {
            get => (MapType)GetValue(MapTypeProperty);
            set => SetValue(MapTypeProperty, value);
        }

        public Position CurrentPosition
        {
            get => (Position)GetValue(CurrentPositionProperty);
            set => SetValue(CurrentPositionProperty, value);
        }

        public double CurrentZoom
        {
            get => (double)GetValue(CurrentZoomProperty);
            set => SetValue(CurrentZoomProperty, value);
        }

        public bool IsRequiredCameraChanging
        {
            get => (bool)GetValue(IsRequiredCameraChangingProperty);
            set => SetValue(IsRequiredCameraChangingProperty, value);
        }

        public double CurrentBearing
        {
            get => (double)GetValue(CurrentBearingProperty);
            set => SetValue(CurrentBearingProperty, value);
        }

        public double CurrentTilt
        {
            get => (double)GetValue(CurrentTiltProperty);
            set => SetValue(CurrentTiltProperty, value);
        }

        public List<Pin> Pins
        {
            get => (List<Pin>)GetValue(PinsProperty);
            set => SetValue(PinsProperty, value);
        }

        public Pin PinAdded
        {
            get => (Pin)GetValue(PinAddedProperty);
            set => SetValue(PinAddedProperty, value);
        }


        #endregion


        #region -- Private helpers --

        private static void MapTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomMap customMap = (CustomMap)bindable;
            customMap.map.MapType = (MapType)newValue;
        }

        private static void CurrentPositionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            _lastPosition = (Position)newValue;
        }

        private static void CurrentZoomPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            _lastZoom = (double)newValue;
        }


        private static void IsRequiredCameraChangingChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if ((bool)newValue)
            {
                CustomMap customMap = (CustomMap)bindable;
                customMap.map.InitialCameraUpdate = CameraUpdateFactory.NewCameraPosition(new CameraPosition(_lastPosition, _lastZoom, _lastBearing, _lastTilt));
            }
        }

        private static void CurrentBearingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            _lastBearing = (double)newValue;
        }


        private static void CurrentTiltPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            _lastTilt = (double)newValue;
        }


        private static void PinsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomMap customMap = (CustomMap)bindable;

            if ((List<Pin>)newValue != null)
                foreach (Pin pin in (List<Pin>)newValue)
                    customMap.map.Pins.Add(pin);
        }


        private void Map_CameraIdled(object sender, CameraIdledEventArgs e)
        {
            CameraPosition cameraPosition = e.Position;
            CurrentPosition = new Position(cameraPosition.Target.Latitude, cameraPosition.Target.Longitude);
            CurrentZoom = cameraPosition.Zoom;
            CurrentBearing = cameraPosition.Bearing;
            CurrentTilt = cameraPosition.Tilt;
        }


        private async void Map_MapClicked(object sender, MapClickedEventArgs e)
        {
            Pin pin = new Pin
            {
                Position = new Position(e.Point.Latitude, e.Point.Longitude)
            };

            Geocoder geocoder = new Geocoder();
            var res = await geocoder.GetAddressesForPositionAsync(pin.Position);
            pin.Label = res != null ? res.FirstOrDefault() : "No name";

            map.Pins.Add(pin);
            PinAdded = pin;
        }

        #endregion


    }
}