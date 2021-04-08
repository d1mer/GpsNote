using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Prism;
using Prism.Ioc;

namespace GpsNote.Droid
{
    [Activity(Theme = "@style/MainTheme",
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        //const int RequestLocationId = 0;
        //readonly string[] LocationPermissions =
        //{
        //    Manifest.Permission.AccessCoarseLocation,
        //    Manifest.Permission.AccessFineLocation
        //};
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            Xamarin.FormsGoogleMaps.Init(this, savedInstanceState);

            LoadApplication(new App(new AndroidInitializer()));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            //if (requestCode == RequestLocationId)
            //{
            //    if ((grantResults.Length == 1) && (grantResults[0] == (int)Permission.Granted))
            //    { 
            //        // Permissions granted - display a message.
            //    }
            //    else
            //    {
            //        // Permissions denied - display a message.
            //    }

            //}
            //else
            //{
            //    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            //}
        }

        //protected override void OnStart()
        //{
        //    base.OnStart();

        //    if ((int)Build.VERSION.SdkInt >= 23)
        //    {
        //        if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted)
        //        {
        //            RequestPermissions(LocationPermissions, RequestLocationId);
        //        }
        //        else
        //        {
        //            // Permissions already granted - display a message.
        //        }
        //    }
        //}
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}

