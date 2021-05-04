using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Google.Android.Material.Tabs;
using GpsNote.Controls;
using GpsNote.Droid;
using GpsNote.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(MainTabbedPage), typeof(ExtendedTabbedPageRenderer))]
namespace GpsNote.Droid
{
    public class ExtendedTabbedPageRenderer : TabbedPageRenderer
    {
        //private TabLayout _TabBar;
        //protected TabLayout TabBar => _TabBar ?? SearchInChildren<TabLayout>();

        //private ViewPager _Pager;
        //protected ViewPager Pager => _Pager ?? SearchInChildren<ViewPager>();

        //protected IVisualElementRenderer _tabBarRenderer;

        public ExtendedTabbedPageRenderer(Context context) : base(context)
        {
        }

        //#region -- Overrides --

        //protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        //{
        //    base.OnElementChanged(e);

        //    if (e.NewElement != null && _tabBarRenderer is null)
        //    {
        //        var tabbedPage = Element as ExtendedTabbedPage;
        //        _tabBarRenderer = Platform.GetRenderer(tabbedPage.TabBarView);

        //        if (_tabBarRenderer is null)
        //        {
        //            _tabBarRenderer = Platform.CreateRendererWithContext(tabbedPage.TabBarView, Context);
        //            Platform.SetRenderer(tabbedPage.TabBarView, _tabBarRenderer);
        //        }

        //        AddView(_tabBarRenderer.View);

        //        TabBar.Visibility = Android.Views.ViewStates.Gone;
        //    }
        //}

        //protected override void OnLayout(bool changed, int l, int t, int r, int b)
        //{
        //    TabBar.Visibility = Android.Views.ViewStates.Gone;

        //    var tabbedPage = Element as ExtendedTabbedPage;

        //    if (tabbedPage.TabBarPosition == ExtendedTabbedPage.TabBarPositionType.Top)
        //    {
        //        base.OnLayout(changed, l, t + (int)(tabbedPage.TabBarHeight * Context.Resources.DisplayMetrics.Density), r, b);

        //        Pager.Layout(l, t + (int)(tabbedPage.TabBarHeight * Context.Resources.DisplayMetrics.Density), r, b);

        //        Xamarin.Forms.Layout.LayoutChildIntoBoundingRegion(_tabBarRenderer.Element,
        //            new Rectangle(l, t, Context.FromPixels(r - l), tabbedPage.TabBarHeight));
        //    }
        //    else
        //    {
        //        base.OnLayout(changed, l, t, r, b - +(int)(tabbedPage.TabBarHeight * Context.Resources.DisplayMetrics.Density));

        //        Pager.Layout(l, t, r, b - +(int)(tabbedPage.TabBarHeight * Context.Resources.DisplayMetrics.Density));

        //        Xamarin.Forms.Layout.LayoutChildIntoBoundingRegion(_tabBarRenderer.Element,
        //            new Rectangle(l, Context.FromPixels(b - t) - tabbedPage.TabBarHeight, Context.FromPixels(r - l), tabbedPage.TabBarHeight));
        //    }

        //    _tabBarRenderer.UpdateLayout();
        //}

        //#endregion

        //#region -- Private Helpers --

        //private T SearchInChildren<T>() where T : class
        //{
        //    T res = null;

        //    for (int i = 0; i < ChildCount; i++)
        //    {
        //        if (GetChildAt(i) is T child)
        //        {
        //            res = child;
        //        }
        //    }

        //    return res;
        //}

        //#endregion
    }
}