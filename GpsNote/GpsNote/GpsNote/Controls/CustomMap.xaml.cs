using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.GoogleMaps;

namespace GpsNote.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomMap : ContentView
    {
        #region -- Private static --

        private static Position _lastPosition;
        private static double _lastZoom;

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
                customMap.map.InitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(_lastPosition, _lastZoom);
            }
        }


        private void Map_CameraIdled(object sender, CameraIdledEventArgs e)
        {
            CameraPosition cameraPosition = e.Position;
            CurrentPosition = new Position(cameraPosition.Target.Latitude, cameraPosition.Target.Longitude);
            CurrentZoom = cameraPosition.Zoom;
        }

        #endregion

    }
}