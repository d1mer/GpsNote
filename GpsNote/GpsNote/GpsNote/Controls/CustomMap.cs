using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GpsNote.Controls
{
    public class CustomMap : Map
    {
        #region -- Public statics --

        public static readonly BindableProperty PinsListProperty =
            BindableProperty.Create(nameof(PinsList),
                                    typeof(List<Pin>),
                                    typeof(CustomMap),
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: PinsListPropertyPropertyChanged);

        public List<Pin> PinsList
        {
            get => (List<Pin>)GetValue(PinsListProperty);
            set => SetValue(PinsListProperty, value);
        }

        #endregion

        #region -- Constructors -- 

        public CustomMap()
        {
            
        }

        #endregion


        #region -- Private helpers --

        private static void PinsListPropertyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomMap map = (CustomMap)bindable;
           
            if ((List<Pin>)newValue != null)
            {
                foreach (Pin pin in (List<Pin>)newValue)
                    map.Pins.Add(pin);
            }
        }

        #endregion
    }
}