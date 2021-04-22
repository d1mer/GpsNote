using GpsNote.Resource;
using GpsNote.Services.SettingsService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Text;
using Xamarin.Forms;

namespace GpsNote.Services.Localization
{
    public class LocalizationService : ILocalizationService, INotifyPropertyChanged
    {
        private readonly ISettingsManager _settingsManager;
        private readonly ResourceManager _resourceManager;
        private CultureInfo _currentCultureInfo;

        public LocalizationService(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _currentCultureInfo = new CultureInfo(_settingsManager.Lang);
            _resourceManager = new ResourceManager(typeof(LangResource));

            MessagingCenter.Subscribe<object, CultureInfo>(this, string.Empty, OnCultureChanged);
        }


        #region -- ILocalizationService implementation --

        public string this[string key]
        {
            get => _resourceManager.GetString(key, _currentCultureInfo);
        }

        public string Lang 
        {
            get => _settingsManager.Lang;
            set => _settingsManager.Lang = value;
        }
       

        public void ChangeCulture(string lang)
        {
            MessagingCenter.Send<object, CultureInfo>(this, string.Empty, new CultureInfo(lang));
        }

        #endregion

        #region -- INotifyPropertyChanged --

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion


        #region -- Private helpers --

        private void OnCultureChanged(object obj, CultureInfo ci)
        {
            _currentCultureInfo = ci;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item"));
        }

        #endregion
    }
}