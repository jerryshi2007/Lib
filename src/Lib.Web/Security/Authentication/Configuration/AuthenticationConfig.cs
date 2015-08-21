using System;
using System.Web.Configuration;
using System.Web;

namespace Lib.Web.Security
{
    public class LibAuthenticationConfig
    {
        private static bool authRequired = true;

        private static string cookiePath;

        private static string cookieName = "LibAuth";

        private static int cookieTimeout = 1;

        private static bool slidingExpiration = true;

        private static bool requireSSL = false;

        //private static string loginUrl = "http://localhost:4868/Pages/Login.aspx";

        private static string cookieDomain ;

        public static bool AuthRequired
        {
            get { return authRequired; }
        }

        public static string CookiePath
        {
            get { return cookiePath; }
        }

        public static string CookieName
        {
            get { return cookieName; }
        }

        /// <summary>
        /// 超时时长，以分钟为单位
        /// </summary>
        public static int CookieTimeout
        {
            get { return cookieTimeout; }
        }

        public static bool SlidingExpiration
        {
            get { return slidingExpiration; }
        }

        public static bool RequireSSL
        {
            get { return requireSSL; }
        }

        public static string CookieDomain
        {
            get { return cookieDomain; }
        }

        public static string LoginUrl
        {
            get
            {
                return AuthenticationClientSection.GetConfig().LoginUrl.AbsolutePath;
            }
        }


        public static string AuthenticationType
        {
            get { return "LibAuth"; }
        }
    }

    public class LibAuthenticationClientConfig : LibAuthenticationConfig
    {
       
    }

    public class LibAuthenticationServerConfig : LibAuthenticationConfig
    { 
    
    }
}
