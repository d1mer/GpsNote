using GpsNote.Services.Localization;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GpsNote.ViewModels
{
    public class SettingsLanguageViewModel : ViewModelBase
    {
        public SettingsLanguageViewModel(INavigationService navigationService, 
                                         ILocalizationService localizationService) : base(navigationService, localizationService)
        {
            
        }

        #region -- Public properties --

        private string languageSelection;
        public string LanguageSelection
        {
            get => languageSelection;
            set => SetProperty(ref languageSelection, value);
        }

        private bool isCheckedEn;
        public bool IsCheckedEn
        {
            get => isCheckedEn;
            set => SetProperty(ref isCheckedEn, value);
        }

        private bool isCheckedRu;
        public bool IsCheckedRu
        {
            get => isCheckedRu;
            set => SetProperty(ref isCheckedRu, value);
        }



        private DelegateCommand backPressedCommand;
        public DelegateCommand BackPressedCommand => backPressedCommand ?? new DelegateCommand(OnBackPressed);

        #endregion


        #region -- Overrides --

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            LanguageSelection = Resource.Lang;

            switch (Resource.Lang)
            {
                case Constants.EN:
                    IsCheckedEn = true ;
                    break;
                case Constants.RU:
                    IsCheckedRu = true;
                    break;
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if(args.PropertyName == nameof(IsCheckedEn))
            {
                LanguageSelection = isCheckedEn ? Constants.EN : Constants.RU;
                Resource.ChangeCulture(LanguageSelection);
                Resource.Lang = LanguageSelection;
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