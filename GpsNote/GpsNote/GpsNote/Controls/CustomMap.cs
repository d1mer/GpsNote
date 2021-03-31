using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace GpsNote.Controls
{
    class CustomMap : Map
    {
        public CustomMap()
        {
            //this.MapType = MapType.Hybrid;
            this.PinClicked += CustomMap_PinClicked;
            this.MapClicked += CustomMap_MapClicked;
            
        }



        private void CustomMap_MapClicked(object sender, MapClickedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void CustomMap_PinClicked(object sender, PinClickedEventArgs e)
        {
            Console.WriteLine();
        }
    }
}