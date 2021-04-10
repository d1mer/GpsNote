using System;
using System.Collections.Generic;
using System.Text;
using Prism.Behaviors;
using Prism.Common;
using Prism.Navigation;
using Xamarin.Forms;

namespace GpsNote.Behaviors
{
    public class TabbedPageNavigationBehavior : BehaviorBase<TabbedPage>
    {
        #region -- Private fields --

        private Page _currentPage;

        #endregion


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