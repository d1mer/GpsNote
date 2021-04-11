using System;
using System.Collections.Generic;
using System.Text;
using GpsNote.Interfaces;
using Unity;
using Unity.Lifetime;
using Prism.Navigation;

namespace GpsNote.ViewModels
{
    public class MainTabbedPageViewModel : ViewModelBase, ICustomTabbedPageSelectedTab
    {
        #region -- Private --

        private readonly IUnityContainer _unityContainer;

        #endregion


        #region -- Constructor --

        public MainTabbedPageViewModel(INavigationService navigationService, IUnityContainer unityContainer) : base(navigationService)
        {
            _unityContainer = unityContainer;

            _unityContainer.RegisterInstance<ICustomTabbedPageSelectedTab>(
                this,
                new ContainerControlledLifetimeManager());
        }

        #endregion


        #region -- Implement ICustomTabbedPageSelectedTab interface --

        private int selectedTab;
        public int SelectedTab 
        { 
            get => selectedTab; 
            set => SetProperty(ref selectedTab, value); 
        }

        public void SetSelectedTab(int tabIndex) => SelectedTab = tabIndex;

        #endregion
    }
}