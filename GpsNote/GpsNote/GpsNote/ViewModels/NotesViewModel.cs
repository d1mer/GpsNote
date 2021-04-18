using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Input;
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

namespace GpsNote.ViewModels
{
    public class NotesViewModel : ViewModelBase, IInitializeAsync
    {
        private IPageDialogService _dialogService;
        private IPinService        _pinService;
        private List<PinViewModel> _oldPinsList = null;


        public NotesViewModel(INavigationService navigationService, 
                              IPinService pinService, 
                              IPageDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
            _pinService = pinService;

            Title = "Notes";
        }


        #region -- Publics --

        private ObservableCollection<PinViewModel> pinsList;
        public ObservableCollection<PinViewModel> PinsList
        {
            get => pinsList;
            set => SetProperty(ref pinsList, value);
        }

        private DelegateCommand addEditPinTapCommand;
        public DelegateCommand AddEditPinTapCommand => addEditPinTapCommand ?? new DelegateCommand(OnAddEditPinAsync);

        private DelegateCommand<object> imageTapCommand;
        public DelegateCommand<object> ImageTapCommand => imageTapCommand ?? 
            new DelegateCommand<Object>(OnChangeVisibilityPinAsync);

        private DelegateCommand<object> deleteTapCommand;
        public DelegateCommand<object> DeleteTapCommand => deleteTapCommand ?? 
            new DelegateCommand<object>(OnDeletePinAsync);

        private DelegateCommand<object> updateTapCommand;
        public DelegateCommand<object> UpdateTapCommand => updateTapCommand ?? 
            new DelegateCommand<object>(OnUpdatePinAsync);

        private DelegateCommand<object> searchTextChangedCommand;
        public DelegateCommand<object> SearchTextChangedCommand => searchTextChangedCommand ?? 
            new DelegateCommand<object>(OnSearchPin);

        private ICommand itemTapCommand;
        public ICommand ItemTapCommand => itemTapCommand ?? new Command(OnItemTapAsync);

        #endregion


        #region -- Overrides --

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.TryGetValue<PinModel>(Constants.NEW_PIN, out PinModel newPin))
            {
                PinViewModel pinViewModel = newPin.ToPinViewModel();
                pinViewModel.ImagePath = pinViewModel.IsEnabled ? "checked.png" : "not_checked.png";

                if (PinsList == null)
                {
                    PinsList = new ObservableCollection<PinViewModel>();
                }

                PinsList.Add(pinViewModel);
            }
            else if (parameters.TryGetValue<PinModel>(Constants.EDIT_PIN, out PinModel editPin))
            {
                PinViewModel pinViewModel = PinsList.FirstOrDefault(p => p.Id == editPin.Id);

                if(pinViewModel != null)
                {
                    int index = PinsList.IndexOf(pinViewModel);
                    PinsList.RemoveAt(index);

                    PinViewModel pinView = editPin.ToPinViewModel();
                    pinView.ImagePath = pinView.IsEnabled ? "checked.jpeg" : "not_checked.png";

                    PinsList.Insert(index, pinView);
                }
            }
        }

        #endregion


        #region -- Implement IInitializeAsync interface --

        public async Task InitializeAsync(INavigationParameters parameters)
        {
            List<PinModel> pinsModel = await _pinService.GetUsersPinsAsync();

            if (pinsModel.Count != 0)
            {
                PinsList = new ObservableCollection<PinViewModel>();
                PinViewModel pinViewModel;

                foreach (PinModel pinModel in pinsModel)
                {
                    pinViewModel = pinModel.ToPinViewModel();
                    pinViewModel.ImagePath = pinModel.IsEnable ? "checked.jpeg" : "not_checked.png";
                    PinsList.Add(pinViewModel);
                }
            }
        }

        #endregion


        #region -- Private helpers --

        private async void OnAddEditPinAsync()
        {
            await NavigationService.NavigateAsync(nameof(NavigationPage) + "/" + nameof(AddEditPinPage));
        }

        private async void OnChangeVisibilityPinAsync(object obj)
        {
            PinViewModel pinViewModel = obj as PinViewModel;

            if (pinViewModel != null)
            {
                pinViewModel.IsEnabled = !pinViewModel.IsEnabled;
                pinViewModel.ImagePath = pinViewModel.IsEnabled ? "checked.jpeg" : "not_checked.png";

                PinModel pinModel = await _pinService.FindPinModelAsync(p => p.Id == pinViewModel.Id);

                if (pinModel != null)
                {
                    pinModel.IsEnable = pinViewModel.IsEnabled;
                    await _pinService.UpdatePinModelAsync(pinModel);
                }
                else
                {
                    await _dialogService.DisplayAlertAsync("Error",
                                                           "This pin is not found in db",
                                                           "Cancel");
                    pinViewModel.IsEnabled = !pinViewModel.IsEnabled;
                    pinViewModel.ImagePath = pinViewModel.IsEnabled ? "checked.jpeg" : "not_checked.png";
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

                await NavigationService.NavigateAsync(nameof(NavigationPage) + "/" + nameof(AddEditPinPage), parameter);
            }
        }

        private async void OnItemTapAsync(object obj)
        {
            PinViewModel pin = obj as PinViewModel;

            if (pin != null)
            {
                Position position = new Position(pin.Latitude, pin.Longitude);
     
                NavigationParameters parameters = new NavigationParameters
                {
                    { $"{Constants.DISPLAY_PIN}", position}
                };
               
                await NavigationService.SelectTabAsync(nameof(MapPage), parameters);
            }            
        }

        private void OnSearchPin(object obj)
        {
            string newText = obj as string;

            if (_oldPinsList == null)
                _oldPinsList = PinsList.ToList();

            if (string.IsNullOrWhiteSpace(newText))
            {
                PinsList = new ObservableCollection<PinViewModel>(_oldPinsList);
                _oldPinsList = null;
            }

            if(PinsList.Count == 0)
            {
                PinsList = new ObservableCollection<PinViewModel>(_oldPinsList);
            }

            var list = PinsList.Where(p => p.Label.Contains(newText, StringComparison.OrdinalIgnoreCase)).ToList();
            PinsList = new ObservableCollection<PinViewModel>(list);
        }

        #endregion
    }
}