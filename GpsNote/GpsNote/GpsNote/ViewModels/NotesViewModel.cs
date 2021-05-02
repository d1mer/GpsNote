using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using GpsNote.Models;
using GpsNote.Services.PinService;
using GpsNote.ViewModels.ExtentedViewModels;
using GpsNote.Extensions;
using GpsNote.Views;
using GpsNote.Services.Localization;

namespace GpsNote.ViewModels
{
    public class NotesViewModel : ViewModelBase
    {
        private IPageDialogService _dialogService;
        private readonly IPinService _pinService;
        private List<PinViewModel> _oldPinsList = null;


        public NotesViewModel(INavigationService navigationService,
                              ILocalizationService localizationService,
                              IPinService pinService, 
                              IPageDialogService dialogService) : base(navigationService, localizationService)
        {
            _pinService = pinService;
            _dialogService = dialogService;

            Title = Resource["PinsTitle"];
            SearchText = string.Empty;
        }


        #region -- Publics --

        private string searchText;
        public string SearchText
        {
            get => searchText;
            set => SetProperty(ref searchText, value);
        }

        private List<PinViewModel> searchResultList;
        public List<PinViewModel> SearchResultList
        {
            get => searchResultList;
            set => SetProperty(ref searchResultList, value);
        }

        private ObservableCollection<PinViewModel> pinsList;
        public ObservableCollection<PinViewModel> PinsList
        {
            get => pinsList;
            set => SetProperty(ref pinsList, value);
        }

        private bool exitSearch;
        public bool ExitSearch
        {
            get => exitSearch;
            set => SetProperty(ref exitSearch, value);
        }

        private ICommand settingsTapCommand;
        public ICommand SettingsTapCommand => settingsTapCommand ?? new DelegateCommand(OnGoToSettings);

        private ICommand logOutTapCommand;
        public ICommand LogOutTapCommand => logOutTapCommand ?? new DelegateCommand(OnLogOut);

        private DelegateCommand addEditPinTapCommand;
        public DelegateCommand AddEditPinTapCommand => addEditPinTapCommand ?? new DelegateCommand(OnAddEditPinAsync);

        private DelegateCommand<object> heartImageTapCommand;
        public DelegateCommand<object> HeartImageTapCommand => heartImageTapCommand ?? 
            new DelegateCommand<Object>(OnChangeVisibilityPinAsync);

        private DelegateCommand<object> arrowImageTapCommand;
        public DelegateCommand<object> ArrowImageTapCommand => arrowImageTapCommand ??
            new DelegateCommand<Object>(OnArrowImageTapAsync);

        private DelegateCommand<object> deleteTapCommand;
        public DelegateCommand<object> DeleteTapCommand => deleteTapCommand ?? 
            new DelegateCommand<object>(OnDeletePinAsync);

        private DelegateCommand<object> updateTapCommand;
        public DelegateCommand<object> UpdateTapCommand => updateTapCommand ?? 
            new DelegateCommand<object>(OnUpdatePinAsync);

        #endregion


        #region -- Overrides --

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<PinModel>(Constants.NEW_PIN, out PinModel newPin))
            {
                PinViewModel pinViewModel = newPin.ToPinViewModel();
                pinViewModel.ImagePath = pinViewModel.IsEnabled ? "ic_like_blue.png" : "ic_like_gray.png";

                if (PinsList == null)
                {
                    PinsList = new ObservableCollection<PinViewModel>();
                }

                PinsList.Add(pinViewModel);
            }
            else if (parameters.TryGetValue<PinModel>(Constants.EDIT_PIN, out PinModel editPin))
            {
                PinViewModel pinViewModel = PinsList.FirstOrDefault(p => p.Id == editPin.Id);

                if (pinViewModel != null)
                {
                    int index = PinsList.IndexOf(pinViewModel);
                    PinsList.RemoveAt(index);

                    PinViewModel pinView = editPin.ToPinViewModel();
                    pinView.ImagePath = pinView.IsEnabled ? "ic_like_blue.png" : "ic_like_gray.png";

                    PinsList.Insert(index, pinView);
                }
            }
        }

        public async override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            PinsList = new ObservableCollection<PinViewModel>();

            List<PinModel> pinsModel = await _pinService.GetUsersPinsAsync();

            if (pinsModel != null && pinsModel.Count != 0)
            {
                PinViewModel pinViewModel;

                foreach (PinModel pinModel in pinsModel)
                {
                    pinViewModel = pinModel.ToPinViewModel();
                    pinViewModel.ImagePath = pinModel.IsEnable ? "ic_like_blue.png" : "ic_like_gray.png";
                    PinsList.Add(pinViewModel);
                }
            }
        }

        protected async override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(SearchText))
            {
                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    ExitSearch = false;
                    OnSearchPin(SearchText);
                }
                else
                {
                    if(_oldPinsList != null)
                    {
                        _oldPinsList = null;
                        PinsList = new ObservableCollection<PinViewModel>();

                        List<PinModel> pinsModel = await _pinService.GetUsersPinsAsync();

                        if (pinsModel != null && pinsModel.Count != 0)
                        {
                            PinViewModel pinViewModel;

                            foreach (PinModel pinModel in pinsModel)
                            {
                                pinViewModel = pinModel.ToPinViewModel();
                                pinViewModel.ImagePath = pinModel.IsEnable ? "ic_like_blue.png" : "ic_like_gray.png";
                                PinsList.Add(pinViewModel);
                            }
                        }
                    }          
                }
            }
        }

        #endregion


        #region -- Private helpers --

        private async void OnAddEditPinAsync()
        {
            await NavigationService.NavigateAsync(nameof(AddEditPinPage));
        }

        private async void OnChangeVisibilityPinAsync(object obj)
        {
            PinViewModel pinViewModel = obj as PinViewModel;

            if (pinViewModel != null)
            {
                pinViewModel.IsEnabled = !pinViewModel.IsEnabled;
                pinViewModel.ImagePath = pinViewModel.IsEnabled ? "ic_like_blue.png" : "ic_like_gray.png";

                PinModel pinModel = await _pinService.FindPinModelAsync(p => p.Id == pinViewModel.Id);

                if (pinModel != null)
                {
                    pinModel.IsEnable = pinViewModel.IsEnabled;
                    await _pinService.UpdatePinModelAsync(pinModel);
                }
                else
                {
                    pinViewModel.IsEnabled = !pinViewModel.IsEnabled;
                    pinViewModel.ImagePath = pinViewModel.IsEnabled ? "ic_like_blue.png" : "ic_like_gray.png";
                }
            }
        }

        private async void OnDeletePinAsync(object obj)
        {
            PinViewModel pinViewModel = obj as PinViewModel;

            if(pinViewModel != null)
            {
                string res = await _dialogService.DisplayActionSheetAsync("Delete selected pin?", "OK", "Cancel");

                if (res == "OK")
                {
                    PinModel pinModel = await _pinService.FindPinModelAsync(p => p.Id == pinViewModel.Id);
                    if(pinModel != null)
                    {
                        int rows = await _pinService.DeletePinModelAsync(pinModel);

                        if(rows != 0)
                        {
                            PinsList.Remove(PinsList.First(p => p.Id == pinViewModel.Id));
                        }
                        else
                        {
                            await _dialogService.DisplayAlertAsync("Error",
                                                                   "Error delete pin from database",
                                                                   "Cancel");
                        }
                    }
                    else
                    {
                        await _dialogService.DisplayAlertAsync("Error",
                                                               "Pin is not found in db",
                                                               "Cancel");
                    }
                }
            }
         }

        private async void OnUpdatePinAsync(object obj)
        {
            PinViewModel pinViewModel = obj as PinViewModel;

            if(pinViewModel != null)
            {
                NavigationParameters parameter = new NavigationParameters
                {
                    {nameof(PinViewModel), pinViewModel }
                };

                await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(AddEditPinPage)}", parameter);
            }
        }

        private void OnSearchPin(object obj)
        {
            string newText = obj as string;

            if (_oldPinsList == null)
            {
                _oldPinsList = PinsList.ToList();
            }

            if(PinsList.Count == 0)
            {
                PinsList = new ObservableCollection<PinViewModel>(_oldPinsList);
            }

            var list = PinsList.Where(p => p.Label.Contains(newText, StringComparison.OrdinalIgnoreCase)).ToList();
            PinsList = new ObservableCollection<PinViewModel>(list);
        }

        private async void OnArrowImageTapAsync(object obj)
        {
            PinViewModel pin = obj as PinViewModel;

            if (pin != null)
            {
                Position position = new Position(pin.Latitude, pin.Longtitude);

                NavigationParameters parameters = new NavigationParameters
                {
                    { $"{Constants.DISPLAY_PIN}", position}
                };

                await NavigationService.SelectTabAsync(nameof(MapPage), parameters);
            }
        }

        private async void OnGoToSettings()
        {
            if (ExitSearch)
            {
                await NavigationService.NavigateAsync(nameof(SettingsPage));
            }
            else if (!ExitSearch)
            {
                ExitSearch = true;
                SearchText = string.Empty;
            }

        }

        private void OnLogOut()
        {
            NavigationService.NavigateAsync($"/{nameof(MainPage)}");
        }

        #endregion
    }
}