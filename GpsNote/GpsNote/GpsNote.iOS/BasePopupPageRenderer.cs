using Foundation;
using GpsNote.iOS;
using GpsNote.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BasePopupPage), typeof(BasePopupPageRenderer))]
namespace GpsNote.iOS
{
    public class BasePopupPageRenderer : PageRenderer
    {
        private UIViewController _parentModalViewController;

        #region -- Overrides --

        public override void DidMoveToParentViewController(UIViewController parent)
        {
            base.DidMoveToParentViewController(parent);
            _parentModalViewController = parent;
            parent.ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            _parentModalViewController.View.BackgroundColor = UIColor.Clear;
            View.BackgroundColor = UIColor.Clear;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            _parentModalViewController.View.BackgroundColor = UIColor.Clear;
            View.BackgroundColor = UIColor.Clear;
        }

        #endregion
    }
}