using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using System;
using GpsNote.Services.GoogleAuthentication.AuthHelpers;


namespace GpsNote.Droid
{
    [Activity(Label = "CustomUrlSchemeInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTask)]
    [IntentFilter(
                  new[] {Intent.ActionView},
                  Categories = new[] {Intent.CategoryDefault, Intent.CategoryBrowsable},
                  DataSchemes = new[] { "com.googleusercontent.apps.347199190339-33q364490p2l7cb84l05umegsbbf47di" },
                  DataPath = "/oauth2redirect")]
    public class CustomUrlSchemeInterceptorActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            global::Android.Net.Uri uri = Intent.Data;

            Uri uriPage = new Uri(uri.ToString());

            AuthenticationState.Authenticator.OnPageLoading(uriPage);

            Intent intent = new Intent(this, typeof(MainActivity));
            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            StartActivity(intent);
            Finish();
        }
    }
}