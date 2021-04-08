using GpsNote.Models;
using GpsNote.Services.RepositoryService;
using GpsNote.Services.SettingsService;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GpsNote.ViewModels
{
    public class NotesViewModel : ViewModelBase, IInitializeAsync
    {
        #region -- Private fields --

        private IRepositoryService _repositoryService;
        private ISettingsService _settingsService;
        private IPageDialogService _dialogService;

        #endregion


        public NotesViewModel(INavigationService navigationService, IRepositoryService repositoryService, ISettingsService settingsService, IPageDialogService dialogService) : base(navigationService)
        {
            _repositoryService = repositoryService;
            _settingsService = settingsService;
            _dialogService = dialogService;
        }



        #region -- Publics --

        private ObservableCollection<PinModel> pinsList;
        public ObservableCollection<PinModel> PinsList
        {
            get => pinsList;
            set => SetProperty(ref pinsList, value);
        }


        #endregion


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

        #region -- Implement IInitializeAsync interface --

        public async Task InitializeAsync(INavigationParameters parameters)
        {
            List<PinModel> pins = null;

            try
            {
                pins = await _repositoryService.GetAllAsync<PinModel>(p => p.Owner == _settingsService.IdCurrentUser);
            }
            catch (Exception ex)
            {
                await _dialogService.DisplayAlertAsync(title: "Error",
                                                 message: ex.Message,
                                                 cancelButton: "Close");
                return;
            }

            if (pins != null)
                PinsList = new ObservableCollection<PinModel>(pins);
        }

        #endregion
    }
}