using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(GpsNote.iOS.CustomTabbedPageRenderer))]
namespace GpsNote.iOS
{
    class CustomTabbedPageRenderer : TabbedRenderer
    {
        //public UIImage ImageWithColor(CGSize size)
        //{
        //    CGRect rect = new CGRect(0, 0, size.Width, size.Height);
        //    UIGraphics.BeginImageContext(size);

        //    using (CGContext context = UIGraphics.GetCurrentContext())
        //    {
        //        //var color = Color.FromHex("F1F3FD").ToCGColor();
        //        context.SetFillColor(new CGColor("F1F3FD"));
        //        context.FillRect(rect);
        //    }

        //    UIImage image = UIGraphics.GetImageFromCurrentImageContext();
        //    UIGraphics.EndImageContext();

        //    return image;
        //}

        //public override void ViewWillAppear(bool animated)
        //{
        //    base.ViewWillAppear(animated);

        //    CGSize size = new CGSize(TabBar.Frame.Width / TabBar.Items.Length, TabBar.Frame.Height);

        //    UITabBar.Appearance.SelectionIndicatorImage = ImageWithColor(size);
        //    UITabBarItem.Appearance.SetTitleTextAttributes(new UITextAttributes { TextColor = UIColor.White }, UIControlState.Normal);
        //    UITabBarItem.Appearance.SetTitleTextAttributes(new UITextAttributes { TextColor = UIColor.Black }, UIControlState.Selected);
        //}
    }
}