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
                                    propertyChanged: OnEntryTextPropertyChanged);


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
                                    propertyChanged: OnErrorTextPropertyChanged);


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
                                    propertyChanged: OnImageSourcePropertyChanged);

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
                                    propertyChanged: OnPlaceholderTextPropertyChanged);


        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }


        #endregion


        #region -- Private helpers --

        private static void OnErrorTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
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
                //else
                //{
                //    customEntry.errorLabel.IsVisible =false;
                //    customEntry.errorLabel.Text = string.Empty;
                //    customEntry.frame.Style = (Xamarin.Forms.Style)App.Current.Resources["frameStyle"];
                //}
                
            }
        }


        private static void OnEntryTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomEntryAuth customEntry = bindable as CustomEntryAuth;

            customEntry.entry.Text = (string)newValue;
        }


        private static void OnImageSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
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


        private static void OnPlaceholderTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
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
                TextEntry = e.NewTextValue;
            }
            else
            {
                image.IsVisible = false;
                errorLabel.IsVisible = false;
                errorLabel.Text = string.Empty;
                frame.Style = (Xamarin.Forms.Style)App.Current.Resources["frameStyle"];
            }
        }

        #endregion

    }
}