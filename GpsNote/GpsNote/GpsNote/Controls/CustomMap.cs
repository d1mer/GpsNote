using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GpsNote.Controls
{
    public class CustomMap : Map
    {
        #region -- Private --

        Position _cameraMovingPosition;

        #endregion


        public CustomMap()
        {
            UiSettings.CompassEnabled = true;
            UiSettings.MyLocationButtonEnabled = true;
        }


        #region -- Public statics --

        public static readonly BindableProperty PinsListProperty =
            BindableProperty.Create(nameof(PinsList),
                                    typeof(List<Pin>),
                                    typeof(CustomMap),
                                    defaultValue: default(List<Pin>),
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: PinsListPropertyPropertyChanged);

        public List<Pin> PinsList
        {
            get => (List<Pin>)GetValue(PinsListProperty);
            set => SetValue(PinsListProperty, value);
        }


        public static readonly BindableProperty IsMoveCameraProperty =
            BindableProperty.Create(nameof(IsMoveCamera),
                                    typeof(bool),
                                    typeof(CustomMap),
                                    defaultValue: false,
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: IsMoveCameraPropertyChanged);


        public bool IsMoveCamera
        {
            get => (bool)GetValue(IsMoveCameraProperty);
            set => SetValue(IsMoveCameraProperty, value);
        }


        public static readonly BindableProperty CameraMovingPositionProperty =
            BindableProperty.Create(nameof(CameraMovingPosition),
                                    typeof(Position),
                                    typeof(CustomMap),
                                    defaultValue: default,
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: CameraMovingPositionPropertyChanged);

        public Position CameraMovingPosition
        {
            get => (Position)GetValue(CameraMovingPositionProperty);
            set => SetValue(CameraMovingPositionProperty, value);
        }


        public static readonly BindableProperty ChangeCompassMyLocationButtonsVisibilityProperty =
            BindableProperty.Create(nameof(ChangeCompassMyLocationButtonsVisibility),
                                    typeof(bool),
                                    typeof(CustomMap),
                                    defaultValue: true,
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: ChangeCompassMyLocationButtonsVisibilityPropertyChanged);


        public bool ChangeCompassMyLocationButtonsVisibility
        {
            get => (bool)GetValue(ChangeCompassMyLocationButtonsVisibilityProperty);
            set => SetValue(ChangeCompassMyLocationButtonsVisibilityProperty, value);
        }

        #endregion


        #region -- Private helpers --

        private static void PinsListPropertyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomMap map = (CustomMap)bindable;
           
            if ((List<Pin>)newValue != null)
            {
                map.Pins.Clear();
                foreach (Pin pin in (List<Pin>)newValue)
                {
                    map.Pins.Add(pin);
                }
            }
        }


        private static void IsMoveCameraPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomMap map = (CustomMap)bindable;

            if((bool)newValue == true)
            {
                map.MoveCamera(CameraUpdateFactory.NewPositionZoom(map._cameraMovingPosition, 10));
                map.IsMoveCamera = false;
            }
        }


        private static void CameraMovingPositionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomMap map = (CustomMap)bindable;
            if (map != null)
            {
                if (newValue is Position)
                    map._cameraMovingPosition = (Position)newValue;
            }
        }


        private static void ChangeCompassMyLocationButtonsVisibilityPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomMap map = bindable as CustomMap;

            if(map != null)
            {
                map.UiSettings.CompassEnabled = (bool)newValue;
                map.UiSettings.MyLocationButtonEnabled = (bool)newValue;
            }
        }

        #endregion
    }
}