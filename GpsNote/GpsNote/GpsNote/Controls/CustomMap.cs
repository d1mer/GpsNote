using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using GpsNote;

namespace GpsNote.Controls
{
    public class CustomMap : Map
    {
        private Position _cameraMovingPosition;

        public CustomMap()
        {
            UiSettings.CompassEnabled = true;
            MyLocationEnabled = false;
            UiSettings.MyLocationButtonEnabled = false;
        }


        #region -- Public properties --

        public static readonly BindableProperty PinsListProperty =
            BindableProperty.Create(nameof(PinsList),
                                    typeof(List<Pin>),
                                    typeof(CustomMap),
                                    defaultValue: default(List<Pin>),
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: PinsListPropertyChanged);

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


        public static readonly BindableProperty ChangeMyLocationButtonVisibilityProperty =
            BindableProperty.Create(nameof(ChangeMyLocationButtonVisibility),
                                    typeof(bool),
                                    typeof(CustomMap),
                                    defaultValue: false,
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: ChangeMyLocationButtonsVisibilityPropertyChanged);


        public bool ChangeMyLocationButtonVisibility
        {
            get => (bool)GetValue(ChangeMyLocationButtonVisibilityProperty);
            set => SetValue(ChangeMyLocationButtonVisibilityProperty, value);
        }

        #endregion


        #region -- Private helpers --

        private static void PinsListPropertyChanged(BindableObject bindable, object oldValue, object newValue)
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
                map.MoveCamera(CameraUpdateFactory.NewPositionZoom(map._cameraMovingPosition, Constants.ZOOM));
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


        private static void ChangeMyLocationButtonsVisibilityPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomMap map = bindable as CustomMap;

            if(map != null)
            {
                map.UiSettings.MyLocationButtonEnabled = (bool)newValue;
                map.MyLocationEnabled = (bool)newValue;
            }
        }

        #endregion
    }
}