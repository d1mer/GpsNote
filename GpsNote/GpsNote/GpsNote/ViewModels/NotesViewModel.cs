using GpsNote.Models;
using GpsNote.Services.PinService;
using GpsNote.Services.SettingsService;
using GpsNote.ViewModels.ExtentedViewModels;
using GpsNote.Extensions;
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

        private ObservableCollection<PinViewModel> pinsList;
        public ObservableCollection<PinViewModel> PinsList
        {
            get => pinsList;
            set => SetProperty(ref pinsList, value);
        }


        private DelegateCommand addEditPinTapCommand;
        public DelegateCommand AddEditPinTapCommand => addEditPinTapCommand ?? (new DelegateCommand(AddEditPin));

        private DelegateCommand<Object> imageTapCommand;
        public DelegateCommand<Object> ImageTapCommand => imageTapCommand ?? (new DelegateCommand<Object>(ChangeVisibilityPinAsync));

        #endregion


        #region -- Overrides --
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            if(parameters.GetNavigationMode() == NavigationMode.Back)
            {
                List<PinModelDb> pinsModelDb;

                try
                {
                    pinsModelDb = _pinService.GetUserPinModels();
                }
                catch (Exception ex)
                {
                    await _dialogService.DisplayAlertAsync("Error",
                                                     ex.Message,
                                                     "Cancel");
                    return;
                }

                if (pinsModelDb != null || pinsModelDb.Count > 0)
                {
                    PinsList = new ObservableCollection<PinViewModel>();
                    foreach(PinModelDb modelDb in pinsModelDb)
                    {
                        PinViewModel pinViewModel = modelDb.PinModelDbToPinViewModel();
                        pinViewModel.ImagePath = modelDb.IsEnable ? "checked.png" : "not_checked.png";
                        PinsList.Add(pinViewModel);
                    }
                    //PinsList = new ObservableCollection<PinViewModel>(pinsModelDb);
                }
            }
        }

        #endregion


        #region -- Implement IInitializeAsync interface --

        public async Task InitializeAsync(INavigationParameters parameters)
        {
            List<PinModelDb> pinsModelDb;

            try
            {
                pinsModelDb = _pinService.GetUserPinModels();
            }
            catch (Exception ex)
            {
                await _dialogService.DisplayAlertAsync(title: "Error",
                                                 message: ex.Message,
                                                 cancelButton: "Close");
                return;
            }

            if (pinsModelDb.Count != 0)
            {
                PinsList = new ObservableCollection<PinViewModel>();
                foreach (PinModelDb modelDb in pinsModelDb)
                {
                    PinViewModel pinViewModel = modelDb.PinModelDbToPinViewModel();
                    pinViewModel.ImagePath = modelDb.IsEnable ? "checked.jpeg" : "not_checked.png";
                    PinsList.Add(pinViewModel);
                }
            }
        }

        #endregion


        #region -- Private helpers --

        private async void AddEditPin()
        {
            await NavigationService.NavigateAsync(nameof(NavigationPage) + "/" + nameof(AddEditPinPage));
        }


        private async void ChangeVisibilityPinAsync(object obj)
        {
            PinViewModel pinViewModel = obj as PinViewModel;

            if (pinViewModel != null)
            {
                pinViewModel.IsEnabled = !pinViewModel.IsEnabled;
                pinViewModel.ImagePath = pinViewModel.IsEnabled ? "checked.jpeg" : "not_checked.png";

                try
                {
                    PinModelDb pinModelDb = await _pinService.FindPinModelDbAsync(p => p.Id == pinViewModel.Id);
                    if(pinModelDb != null)
                    {
                        pinModelDb.IsEnable = pinViewModel.IsEnabled;
                        await _pinService.UpdatePinModelDbAsync(pinModelDb);
                    }
                    //int res = await _pinService.UpdatePinModelDbAsync(pinViewModel);
                    //await _dialogService.DisplayAlertAsync("Error",
                    //                                 res.ToString(),
                    //                                 "Cancel");
                }
                catch(Exception ex)
                {
                    await _dialogService.DisplayAlertAsync("Error",
                                                     ex.Message,
                                                     "Cancel");
                    pinViewModel.IsEnabled = !pinViewModel.IsEnabled;
                    pinViewModel.ImagePath = pinViewModel.IsEnabled ? "checked.jpeg" : "not_checked.png";
                    return;
                }
            }
        }

        #endregion
    }
}