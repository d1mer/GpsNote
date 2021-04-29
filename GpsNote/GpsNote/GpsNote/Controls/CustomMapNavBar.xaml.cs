using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Prism.Ioc;
using Prism.Navigation;
using GpsNote.Views;
using GpsNote.Services.Authorization;
using System.Windows.Input;
using System.ComponentModel;

namespace GpsNote.Controls
{
    public partial class CustomMapNavBar : ContentView
    {
        public CustomMapNavBar()
        {
            InitializeComponent();
        }

        #region -- Public properties --

        public static readonly BindableProperty SearchTextProperty =
            BindableProperty.Create(nameof(SearchText),
                                    typeof(string),
                                    typeof(CustomMapNavBar),
                                    defaultValue: string.Empty,
                                    defaultBindingMode: BindingMode.OneWayToSource,
                                    propertyChanged: SearchTextPropertyChanged);


        public string SearchText
        {
            get => (string)GetValue(SearchTextProperty);
            set => SetValue(SearchTextProperty, value);
        }


        public static readonly BindableProperty LeftIconTapCommandProperty =
            BindableProperty.Create(nameof(LeftIconTapCommand),
                                    typeof(ICommand),
                                    typeof(CustomMapNavBar),
                                    defaultValue: default(ICommand),
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: LeftIconTapCommandPropertyChanged);


        public ICommand LeftIconTapCommand
        {
            get => (ICommand)GetValue(LeftIconTapCommandProperty);
            set => SetValue(LeftIconTapCommandProperty, value);
        }


        public static readonly BindableProperty RightIconTapCommandProperty =
            BindableProperty.Create(nameof(RightIconTapCommand),
                                    typeof(ICommand),
                                    typeof(CustomMapNavBar),
                                    defaultValue: default(ICommand),
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: RightIconTapCommandPropertyChanged);

        public ICommand RightIconTapCommand
        {
            get => (ICommand)GetValue(RightIconTapCommandProperty);
            set => SetValue(RightIconTapCommandProperty, value);
        }


        public static readonly BindableProperty ExitSearchProperty =
            BindableProperty.Create(nameof(ExitSearch),
                                    typeof(bool),
                                    typeof(CustomMapNavBar),
                                    defaultValue: true,
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: ExitSearchPropertyChanged);

        public bool ExitSearch
        {
            get => (bool)GetValue(ExitSearchProperty);
            set => SetValue(ExitSearchProperty, value);
        }

        #endregion


        #region -- Private helpers --

        private static void LeftIconTapCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomMapNavBar navBar = bindable as CustomMapNavBar;

            if (navBar != null)
            {
                navBar.LeftIconTapCommand = (ICommand)newValue;
            }
        }

        private static void RightIconTapCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomMapNavBar navBar = bindable as CustomMapNavBar;

            if (navBar != null)
            {
                navBar.RightIconTapCommand = (ICommand)newValue;
            }
        }

        private static void ExitSearchPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomMapNavBar navBar = bindable as CustomMapNavBar;

            if(navBar != null)
            {
                if ((bool)newValue)
                {
                    navBar.logOutButton.IsVisible = true;
                    Grid.SetColumnSpan(navBar.searchBar, 1);
                    navBar.grid.HorizontalOptions = LayoutOptions.FillAndExpand;
                    navBar.grid.Padding = new Thickness(0, 7, 0, 0);
                    navBar.imageClear.IsVisible = false;
                    navBar.settingsButton.Source = "ic_settings.png";
                    navBar.searchEntry.Text = string.Empty;
                }
            }
        }

        private void SearchBar_Focused(object sender, FocusEventArgs e)
        {
            ExitSearch = false;
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


        private void SearchEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(SearchText == null)
            {
                SearchText = string.Empty;
;           }

            SearchText = (string)e.NewTextValue;
        }


        private static void SearchTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomMapNavBar navBar = bindable as CustomMapNavBar;

            if(navBar != null)
            {
                navBar.SearchText = (string)newValue;
            }
        }

        #endregion
    }
}