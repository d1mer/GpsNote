using GpsNote.Models;
using GpsNote.Services.PinService;
using GpsNote.Services.RepositoryService;
using GpsNote.Services.SettingsService;
using GpsNote.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GpsNote.ViewModels
{
    public class NotesViewModel : ViewModelBase, IInitializeAsync
    {
        #region -- Private fields --

        private ISettingsService   _settingsService;
        private IPageDialogService _dialogService;
        private IPinService        _pinService;

        #endregion


        #region -- Constructor --

        public NotesViewModel(INavigationService navigationService, IPinService pinService, ISettingsService settingsService, IPageDialogService dialogService) : base(navigationService)
        {
            _settingsService = settingsService;
            _dialogService = dialogService;
            _pinService = pinService;
        }

        #endregion


        #region -- Publics --

        private ObservableCollection<PinModelDb> pinsList;
        public ObservableCollection<PinModelDb> PinsList
        {
            get => pinsList;
            set => SetProperty(ref pinsList, value);
        }


        private DelegateCommand addEditPinTapCommand;
        public DelegateCommand AddEditPinTapCommand => addEditPinTapCommand ?? (new DelegateCommand(AddEditPin));


        #endregion


        #region -- Overrides --
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            if(parameters.GetNavigationMode() == NavigationMode.Back)
            {
                List<PinModelDb> pins;

                try
                {
                    pins = _pinService.GetUserPinModels();
                }
                catch (Exception ex)
                {
                    await _dialogService.DisplayAlertAsync("Error",
                                                     ex.Message,
                                                     "Cancel");
                    return;
                }

                if (pins != null)
                    PinsList = new ObservableCollection<PinModelDb>(pins);
            }
        }

        #endregion


        #region -- Implement IInitializeAsync interface --

        public async Task InitializeAsync(INavigationParameters parameters)
        {
            List<PinModelDb> pins;

            try
            {
                pins = _pinService.GetUserPinModels();
            }
            catch (Exception ex)
            {
                await _dialogService.DisplayAlertAsync(title: "Error",
                                                 message: ex.Message,
                                                 cancelButton: "Close");
                return;
            }

            if (pins != null)
                PinsList = new ObservableCollection<PinModelDb>(pins);
        }

        #endregion


        #region -- Private helpers --

        private async void AddEditPin()
        {
            await NavigationService.NavigateAsync(nameof(NavigationPage) + "/" + nameof(AddEditPinPage));
        }

        #endregion
    }
}