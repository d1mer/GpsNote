using Prism.Commands;
using Prism.Navigation;
using GpsNote.Services.Color;
using GpsNote.Services.Localization;

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
        }


        #region -- Public properties --

        private object colorSelection;
        public object ColorSelection
        {
            get => colorSelection;
            set => SetProperty(ref colorSelection, value);
        }

        private DelegateCommand backPressedCommand;
        public DelegateCommand BackPressedCommand => backPressedCommand ?? new DelegateCommand(OnBackPressed);

        #endregion


        #region -- Overrides --

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            int color = _colorService.GetCurrentColor();

            switch (color)
            {
                case 0:
                    ColorSelection = "Blue";
                    break;
                case 1:
                    ColorSelection = "Red";
                    break;
                case 2:
                    ColorSelection = "Green";
                    break;
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            string color = ColorSelection.ToString();
            int value = color == "Blue" ? 0 : color == "Red" ? 1 : 2;
            _colorService.SaveCurrentColor((Enums.Colors)value);
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