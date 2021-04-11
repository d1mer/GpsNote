using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GpsNote.Controls.CustomTabbedPage
{
    public class CustomTabbedPage : TabbedPage
    {
        #region -- Private --

        private bool _isTabPageVisible;

        #endregion


        public CustomTabbedPage()
        {
            
        }


        #region -- Public statics --

        public static readonly BindableProperty SelectedTabIndexProperty =
            BindableProperty.Create(nameof(SelectedTabIndex),
                                    typeof(int),
                                    typeof(CustomTabbedPage),
                                    defaultValue: 0,
                                    defaultBindingMode: BindingMode.TwoWay,
                                    null,
                                    propertyChanged: OnSelectedTabIndexChanged);


        public int SelectedTabIndex
        {
            get => (int)GetValue(SelectedTabIndexProperty);
            set => SetValue(SelectedTabIndexProperty, value);
        }

        #endregion


        #region -- Override --

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _isTabPageVisible = true;
            CurrentPage = Children[SelectedTabIndex];
        }


        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            _isTabPageVisible = false;
        }


        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();

            SelectedTabIndex = Children.IndexOf(CurrentPage);
        }

        #endregion


        #region -- Private helpers --

        private static void OnSelectedTabIndexChanged(BindableObject bindable, object oldValue, object newValue)
        {
            CustomTabbedPage tabbedPage = bindable as CustomTabbedPage;

            if(tabbedPage != null)
            {
                if (tabbedPage._isTabPageVisible)
                {
                    tabbedPage.CurrentPage = tabbedPage.Children[(int)newValue];
                }
            }
        }

        #endregion
    }
}