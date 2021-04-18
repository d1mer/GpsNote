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
    }
}