using System.Web;

namespace Lib.Web.Security
{
    public sealed class LibAuthentication
    {
        public static void RenewTicket(ILibAuthenticationTicket ticket)
        {
            ticket = new LibAuthenticationTicket(ticket);
        }

        public static string Encrypt(ILibAuthenticationTicket ticket)
        {
            return ticket.Serialize();
        }

        public static ILibAuthenticationTicket Decrypt(string encryptedTicket)
        {
            ILibAuthenticationTicket ticket = new LibAuthenticationTicket();
            return ticket.Deserialize(encryptedTicket);
        }

        public static ILibAuthenticationTicket ExtractTicketFromCookie(ref bool fromCookie)
        {
            ILibAuthenticationTicket ticket = null;

            if (fromCookie)
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[LibAuthenticationConfig.CookieName];
                if (cookie != null)
                {
                    ticket = LibAuthentication.Decrypt(cookie.Value);
                    fromCookie = true;
                }
                else
                    fromCookie = false;
            }

            return ticket;
        }

        public static ILibAuthenticationTicket PrepareTicket(ILibAuthenticationTicket ticket)
        {
            if (ticket != null && !ticket.Expired)
            {
                if (LibAuthenticationConfig.SlidingExpiration)
                {
                    LibAuthentication.RenewTicket(ticket);
                }
            }

            return ticket;
        }

        public static LibAuthenticationTicket CreateTicket(ILoginUser user)
        {
            return new LibAuthenticationTicket(user, true);
        }

        public static HttpCookie PrepareCookie(ILibAuthenticationTicket ticket, bool fromCookie)
        {
            HttpCookie cookie = null;

            if (fromCookie && !LibAuthenticationConfig.CookiePath.Equals("/"))
            {
                cookie = HttpContext.Current.Request.Cookies[LibAuthenticationConfig.CookieName];
                if (cookie != null)
                {
                    cookie.Path = LibAuthenticationConfig.CookiePath;
                }
            }

            if (cookie == null)
            {
                cookie = new HttpCookie(LibAuthenticationConfig.CookieName);
                cookie.Path = LibAuthenticationConfig.CookiePath;
            }
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.LoginTimeout;
            }
            cookie.Value = LibAuthentication.Encrypt(ticket);
            cookie.Secure = LibAuthenticationConfig.RequireSSL;
            cookie.HttpOnly = true;

            if (LibAuthenticationConfig.CookieDomain != null)
            {
                cookie.Domain = LibAuthenticationConfig.CookieDomain;
            }

            return cookie;
        }

        public static bool AccessingLoginPage(HttpContext context, string loginUrl)
        {
            if (context.Request.Url.AbsolutePath.CompareTo(loginUrl) == 0)
                return true;

            return false;
        }

        public static void RedirectLogin(HttpContext context)
        {
            string login = string.Format("{0}?ReturnUrl={1}", LibAuthenticationConfig.LoginUrl, HttpUtility.UrlEncode(context.Request.RawUrl));
            context.Response.Redirect(login);
        }

        public static void SetAuthCookie(ILoginUser user)
        {
            if (!HttpContext.Current.Request.IsSecureConnection && LibAuthenticationConfig.RequireSSL)
            {
                throw new HttpException("Connection_not_secure_creating_secure_cookie");
            }

            bool fromCookie = false;

            ILibAuthenticationTicket ticket = LibAuthentication.ExtractTicketFromCookie(ref fromCookie);
            if (ticket == null)
                ticket = LibAuthentication.CreateTicket(user);
            HttpCookie cookie = LibAuthentication.PrepareCookie(ticket, false);
            if (fromCookie)
            {
                HttpContext.Current.Response.Cookies.Remove(cookie.Name);

                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            else
            {
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public static ILoginUser Authentication(string userName, string password)
        {
            ILoginUser user = LoginUser.CreateLoginUser(userName);
            ILoginUser checkUser = user.Check(password);
            if (checkUser == null)
                return user;

            SetAuthCookie(checkUser);

            return checkUser;
        }
    }
}
