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
using System.Linq;
using System.Windows.Input;
using Unity;
using GpsNote.Interfaces;
using Xamarin.Forms.GoogleMaps;

namespace GpsNote.ViewModels
{
    public class NotesViewModel : ViewModelBase, IInitializeAsync
    {
        #region -- Private fields --

        private ISettingsService   _settingsService;
        private IPageDialogService _dialogService;
        private IPinService        _pinService;
        private IUnityContainer _unityContainer;
        private PinViewModel _pinForDisplaying;

        #endregion


        #region -- Constructor --

        public NotesViewModel(INavigationService navigationService, IPinService pinService, ISettingsService settingsService, IPageDialogService dialogService, IUnityContainer unityContainer) : base(navigationService)
        {
            _settingsService = settingsService;
            _dialogService = dialogService;
            _pinService = pinService;
            _unityContainer = unityContainer;

            Title = "Notes";
        }

        #endregion


        #region -- Publics --

        private ObservableCollection<PinViewModel> pinsList;
        public ObservableCollection<PinViewModel> PinsList
        {
            get => pinsList;
            set => SetProperty(ref pinsList, value);
        }

        public Element ParentPage
        {
            get;set;
        }


        private DelegateCommand addEditPinTapCommand;
        public DelegateCommand AddEditPinTapCommand => addEditPinTapCommand ?? (new DelegateCommand(AddEditPinAsync));

        private DelegateCommand<object> imageTapCommand;
        public DelegateCommand<object> ImageTapCommand => imageTapCommand ?? (new DelegateCommand<Object>(ChangeVisibilityPinAsync));

        private DelegateCommand<object> deleteTapCommand;
        public DelegateCommand<object> DeleteTapCommand => deleteTapCommand ?? (new DelegateCommand<object>(DeletePinAsync));

        private DelegateCommand<object> updateTapCommand;
        public DelegateCommand<object> UpdateTapCommand => updateTapCommand ?? (new DelegateCommand<object>(UpdateTapAsync));

        private ICommand itemTapCommand;
        public ICommand ItemTapCommand => itemTapCommand ?? (new Command(ItemTap));

        #endregion


        #region -- Overrides --

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<PinModelDb>("NewPin", out PinModelDb newPin))
            {
                PinViewModel pinViewModel = newPin.PinModelDbToPinViewModel();
                pinViewModel.ImagePath = pinViewModel.IsEnabled ? "checked.png" : "not_checked.png";
                if (PinsList == null)
                    PinsList = new ObservableCollection<PinViewModel>();
                PinsList.Add(pinViewModel);
            }
            else if (parameters.TryGetValue<PinModelDb>("EditPin", out PinModelDb editPin))
            {
                PinViewModel pinViewModel = PinsList.FirstOrDefault(p => p.Id == editPin.Id);
                if(pinViewModel != null)
                {
                    int index = PinsList.IndexOf(pinViewModel);
                    PinsList.RemoveAt(index);

                    PinViewModel pinView = editPin.PinModelDbToPinViewModel();
                    pinView.ImagePath = pinView.IsEnabled ? "checked.jpeg" : "not_checked.png";

                    PinsList.Insert(index, pinView);
                }
            }
        }


        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            if(_pinService.IsDisplayConcretePin == true)
            {
                Position position = new Position(_pinForDisplaying.Latitude, _pinForDisplaying.Longitude);
                _pinForDisplaying = null;
                parameters.Add("displayPin", position);
            }
        }

        #endregion


        #region -- Implement IInitializeAsync interface --

        public async Task InitializeAsync(INavigationParameters parameters)
        {
            List<PinModelDb> pinsModelDb;

            try
            {
                pinsModelDb = await _pinService.GetUserPinModelsFromDatabaseAsync();
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

        private async void AddEditPinAsync()
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
                    //await _pinService.UpdatePinModelDbAsync(pinViewModel);
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


        private async void DeletePinAsync(object obj)
        {
            PinViewModel pinViewModel = obj as PinViewModel;

            if(pinViewModel != null)
            {
                string res = await _dialogService.DisplayActionSheetAsync("Delete selected pin?", "OK", "Cancel");
                if (res != "OK")
                    return;

                PinsList.Remove(PinsList.First(p => p.Id == pinViewModel.Id));

                try
                {
                    await _pinService.DeletePinModelDbAsync(pinViewModel);
                }
                catch (Exception ex)
                {
                    await _dialogService.DisplayAlertAsync("Error",
                                                     ex.Message,
                                                     "Cancel");
                }
            }
         }


        private async void UpdateTapAsync(object obj)
        {
            PinViewModel pinViewModel = obj as PinViewModel;

            if(pinViewModel != null)
            {
                NavigationParameters parameter = new NavigationParameters
                {
                    {"pin", pinViewModel }
                };

                await NavigationService.NavigateAsync(nameof(NavigationPage) + "/" + nameof(AddEditPinPage), parameter);
            }
        }


        private void ItemTap(object obj)
        {
            _pinForDisplaying = obj as PinViewModel;

            if (_pinForDisplaying == null)
                return;

            _pinService.IsDisplayConcretePin = true;
            _unityContainer.Resolve<ICustomTabbedPageSelectedTab>().SetSelectedTab(0);
        }

        #endregion
    }
}