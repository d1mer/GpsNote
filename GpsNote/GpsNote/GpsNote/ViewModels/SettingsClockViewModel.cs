using GpsNote.Services.Color;
using GpsNote.Services.Localization;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GpsNote.ViewModels
{
    public class SettingsClockViewModel : ViewModelBase
    {
        IColorService _colorService;

        public SettingsClockViewModel(INavigationService navigationService,
                                      ILocalizationService localizationService,
                                      IColorService colorService) : base(navigationService, localizationService)
        {
            _colorService = colorService;

            Title = Resource["SettingsClockTitle"];
        }


        #region -- Public properties --

        private object colorSelection;
        public object ColorSelection
        {
            get => colorSelection;
            set => SetProperty(ref colorSelection, value);
        }

        private string clockColor;
        public string ClockColor
        {
            get => clockColor;
            set => SetProperty(ref clockColor, value);
        }

        private DelegateCommand backPressedCommand;
        public DelegateCommand BackPressedCommand => backPressedCommand ?? new DelegateCommand(OnBackPressed);

        #endregion


        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if(args.PropertyName == nameof(ColorSelection))
            {

            }
        }

        #endregion


        #region -- Private helpers --

        private void OnBackPressed()
        {
            NavigationService.GoBackAsync();
        }

        #endregion
    }
}