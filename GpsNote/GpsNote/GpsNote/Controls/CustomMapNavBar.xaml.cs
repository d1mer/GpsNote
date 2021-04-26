using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Prism.Ioc;
using Prism.Navigation;
using GpsNote.Views;
using GpsNote.Services.Authorization;

namespace GpsNote.Controls
{
    public partial class CustomMapNavBar : ContentView
    {
        private bool search = false;
        private INavigationService _navigationService;
        private IAuthorizationService _authorizationService;

        public CustomMapNavBar()
        {
            InitializeComponent();

            SetServices();
        }

        #region -- Public properties --

        public static readonly BindableProperty SearchTextProperty =
            BindableProperty.Create(nameof(SearchText),
                                    typeof(string),
                                    typeof(CustomMapNavBar),
                                    defaultValue: string.Empty,
                                    defaultBindingMode: BindingMode.TwoWay/*,
                                    propertyChanged: SearchTextPropertyChanged*/);

        public string SearchText
        {
            get => (string)GetValue(SearchTextProperty);
            set => SetValue(SearchTextProperty, value);
        }


        public static readonly BindableProperty SearchResultListProperty =
            BindableProperty.Create(nameof(SearchResultList),
                                    typeof(ObservableCollection<Pin>),
                                    typeof(CustomMapNavBar),
                                    defaultValue: default(ObservableCollection<Pin>),
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: SearchResultListPropertyChanged);

        public ObservableCollection<Pin> SearchResultList
        {
            get => (ObservableCollection<Pin>)GetValue(SearchResultListProperty);
            set => SetValue(SearchResultListProperty, value);
        }

        #endregion

        #region -- Private helpers --

        private async void SettingsButton_Clicked(object sender, EventArgs e)
        {
            if (search)
            {
                logOutButton.IsVisible = true;
                Grid.SetColumnSpan(searchBar, 1);
                grid.HorizontalOptions = LayoutOptions.FillAndExpand;
                grid.Padding = new Thickness(0, 7, 0, 0);
                imageClear.IsVisible = false;
                settingsButton.Source = "ic_settings.png";
                searchEntry.Text = string.Empty;
                search = false;
            }
            else
            {
                await _navigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(SettingsPage)}");
            }
        }

        private void LogOutButton_Clicked(object sender, EventArgs e)
        {
            _authorizationService.LogOut();
            _navigationService.NavigateAsync($"/{nameof(MainPage)}");
        }

        private void SearchBar_Focused(object sender, FocusEventArgs e)
        {
            search = true;
            logOutButton.IsVisible = false;
            Grid.SetColumnSpan(searchBar, 2);
            grid.Padding = new Thickness(0, 7, 15, 3);
            imageClear.IsVisible = true;
            settingsButton.Source = "ic_left_blue.png";
        }

        private void ImageClear_Tapped(object sender, EventArgs e)
        {
            searchEntry.Text = string.Empty;
        }

        //private static void SearchTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        //{
        //    CustomMapNavBar navbar = bindable as CustomMapNavBar;

        //    if(navbar != null)
        //    {
        //        navbar.searchEntry.Text = navbar.SearchText;
        //    }
        //}

        private void SearchEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                searchResultList.IsVisible = false;
            }

            SearchText = e.NewTextValue;
        }

        private static void SearchResultListPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomMapNavBar navbar = bindable as CustomMapNavBar;

            if(navbar != null)
            {
                navbar.searchResultList.IsVisible = true;
                navbar.searchResultList.HeightRequest = navbar.searchResultList.RowHeight * navbar.SearchResultList.Count;
            }
        }

        private void SetServices()
        {
            if(_navigationService == null)
            {
                App app = (App)Application.Current;
                _navigationService = app.Container.Resolve<INavigationService>();
                _authorizationService = app.Container.Resolve<IAuthorizationService>();
            }
        }

        #endregion
    }
}