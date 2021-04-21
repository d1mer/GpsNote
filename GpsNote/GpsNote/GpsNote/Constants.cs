using System;
using System.Collections.Generic;
using System.Text;

namespace GpsNote
{
    public static class Constants
    {
        // Pin constants
        public const string EDIT_PIN = "EditPin";
        public const string NEW_PIN = "NewPin";
        public const string DISPLAY_PIN = "DisplayPin";


        // Authorization
        public const string NEW_USER_EMAIL = "NewUserEmail";


        // Camera settings
        public const int ZOOM = 5;
        public const int INITIAL_ZOOM = 2;


        // Google authentication
        public const string APP_NAME = "GpsNotes";
        public const string IOS_CLIENT_ID = "347199190339-e1jfkfrgdcqfcu9667aapvlb3kpm2be9.apps.googleusercontent.com";
        public const string ANDROID_CLIENT_ID = "347199190339-33q364490p2l7cb84l05umegsbbf47di.apps.googleusercontent.com";
        public const string SCOPE = "https://www.googleapis.com/auth/userinfo.email";
        public const string AUTHORIZE_URL = "https://accounts.google.com/o/oauth2/auth";
        public const string ACCESS_TOKEN_URL = "https://www.googleapis.com/oauth2/v4/token";
        public const string USER_INFO_URL = "https://www.googleapis.com/oauth2/v2/userinfo";
        public const string IOS_REDIRECT_URL = "com.googleusercontent.apps.347199190339-e1jfkfrgdcqfcu9667aapvlb3kpm2be9:/oauth2redirect";
        public const string ANDROID_REDIRECT_URL = "com.googleusercontent.apps.347199190339-33q364490p2l7cb84l05umegsbbf47di:/oauth2redirect";
        public const string AUTHENTICATION_RESULT_OK = "OK";

        // Google time zone API
        public const string GPSNOTE_KEY = "AIzaSyBqqk9aQg8BMyPcApsArXclgqkXNXmHf28";
        public const string BASE_URI_TIMEZONE_API = "https://maps.googleapis.com/maps/api/timezone/json?";

        // Popups
        public const string TUPLE = "tuple";

        // Clock
        public const string GRAY_FILL_CLOCK_COLOR = "#F1F3FD";
        public const string LIGHT_BLUE_COLOR = "#C7CDF5";
        public const string DARK_BLUE_COLOR = "#596EFB";
        public const string LIGHT_RED_COLOR = "#F5C7C7";
        public const string DARK_RED_COLOR = "#DD4949";
        public const string LIGHT_GREEN_COLOR = "#00FFB1";
        public const string DARK_GREEN_COLOR = "#32D27F";
    }
}