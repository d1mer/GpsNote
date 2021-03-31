using GpsNote.Controls;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace GpsNote.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        
        public MapViewModel(INavigationService navigationService) : base(navigationService)
        {
            
        }

        //private CustomMap map = new CustomMap();
        //public Map Map
        //{
        //    get => map;
        //    set => SetProperty(ref map, value);
        //}
    }
}
