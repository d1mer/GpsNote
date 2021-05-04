using System;
using Xamarin.Forms;
using Prism.Behaviors;
using Prism.Common;
using Prism.Navigation;

namespace GpsNote.Behaviors
{
    public class TabbedPageNavigationBehavior : BehaviorBase<TabbedPage>
    {
        private Page _currentPage;

        #region -- Override --

        protected override void OnAttachedTo(TabbedPage bindable)
        {
            bindable.CurrentPageChanged += this.OnCurrentPageChanged;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(TabbedPage bindable)
        {
            bindable.CurrentPageChanged -= this.OnCurrentPageChanged;
            base.OnDetachingFrom(bindable);
        }

        #endregion


        #region -- Private helpers --

        private void OnCurrentPageChanged(object sender, EventArgs e)
        {
            Page newPage = this.AssociatedObject.CurrentPage;

            if(_currentPage != null)
            {
                NavigationParameters parameters = new NavigationParameters();
                PageUtilities.OnNavigatedFrom(_currentPage, parameters);
                PageUtilities.OnNavigatedTo(newPage, parameters);
            }

            _currentPage = newPage;
        }

        #endregion
    }
}