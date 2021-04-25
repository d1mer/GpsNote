using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GpsNote.Controls
{
    public partial class CustomEntryAuth : ContentView
    {
        public CustomEntryAuth()
        {
            InitializeComponent();
        }


        #region -- Public properties --

        public static readonly BindableProperty TextEntryProperty =
            BindableProperty.Create(nameof(TextEntry),
                                    typeof(string),
                                    typeof(CustomEntryAuth),
                                    defaultValue: string.Empty,
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: TextEntryPropertyChanged);


        public string TextEntry
        {
            get => (string)GetValue(TextEntryProperty);
            set => SetValue(TextEntryProperty, value);
        }


        public static readonly BindableProperty ErrorTextProperty =
            BindableProperty.Create(nameof(ErrorText),
                                    typeof(string),
                                    typeof(CustomEntryAuth),
                                    defaultValue: string.Empty,
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: ErrorTextPropertyChanged);


        public string ErrorText
        {
            get => (string)GetValue(ErrorTextProperty);
            set => SetValue(ErrorTextProperty, value);
        }

        public static readonly BindableProperty ImageSourceProperty =
            BindableProperty.Create(nameof(ImageSource),
                                    typeof(string),
                                    typeof(CustomEntryAuth),
                                    defaultValue: string.Empty,
                                    propertyChanged: ImageSourcePropertyChanged);

        public string ImageSource
        {
            get => (string)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }


        public static readonly BindableProperty PlaceholderTextProperty =
            BindableProperty.Create(nameof(PlaceholderText),
                                    typeof(string),
                                    typeof(CustomEntryAuth),
                                    defaultValue: string.Empty,
                                    propertyChanged: PlaceholderTextPropertyChanged);


        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }


        public static readonly BindableProperty ShowPasswordProperty =
            BindableProperty.Create(nameof(ShowPassword),
                                    typeof(bool),
                                    typeof(CustomEntryAuth),
                                    defaultValue: true,
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: ShowPasswordPropertyChanged);


        public bool ShowPassword
        {
            get => (bool)GetValue(ShowPasswordProperty);
            set => SetValue(ShowPasswordProperty, value);
        }


        public static readonly BindableProperty TapImageProperty =
            BindableProperty.Create(nameof(TapImage),
                                    typeof(bool),
                                    typeof(CustomEntryAuth),
                                    defaultValue: false,
                                    defaultBindingMode: BindingMode.TwoWay);


        public bool TapImage
        {
            get => (bool)GetValue(TapImageProperty);
            set => SetValue(TapImageProperty, value);
        }


        #endregion


        #region -- Private helpers --

        private static void ErrorTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomEntryAuth customEntry = bindable as CustomEntryAuth;

            if(customEntry != null)
            {
                if (!string.IsNullOrWhiteSpace((string)newValue))
                {
                    customEntry.errorLabel.IsVisible = true;
                    customEntry.errorLabel.Text = (string)newValue;
                    customEntry.frame.Style = (Xamarin.Forms.Style)App.Current.Resources["frameErrorAlert"];
                }
                else
                {
                    customEntry.errorLabel.IsVisible = false;
                    customEntry.errorLabel.Text = string.Empty;
                    customEntry.frame.Style = (Xamarin.Forms.Style)App.Current.Resources["frameStyle"];
                }

            }
        }


        private static void TextEntryPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomEntryAuth customEntry = bindable as CustomEntryAuth;
            string value = (string)newValue;

            if (customEntry != null)
            {
                customEntry.entry.Text = value;
                customEntry.errorLabel.Text = string.Empty;
            }    
        }


        private static void ImageSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomEntryAuth customEntry = bindable as CustomEntryAuth;

            if(customEntry != null)
            {
                if (!string.IsNullOrWhiteSpace((string)newValue))
                {
                    customEntry.image.Source = (string)newValue;
                }
            }
        }


        private static void PlaceholderTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomEntryAuth customEntry = bindable as CustomEntryAuth;

            if(customEntry != null)
            {
                customEntry.entry.Placeholder = (string)newValue;
            }
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                image.IsVisible = true;
            }
            else
            {
                image.IsVisible = false;
                errorLabel.IsVisible = false;
                errorLabel.Text = string.Empty;
                frame.Style = (Xamarin.Forms.Style)App.Current.Resources["frameStyle"];
            }

            TextEntry = e.NewTextValue;
        }

        private static void ShowPasswordPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomEntryAuth customEntry = bindable as CustomEntryAuth;

            if(customEntry != null)
            {
                customEntry.entry.IsPassword = !(bool)newValue;
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            TapImage = !TapImage;
        }

        #endregion
    }
}