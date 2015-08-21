using System;
using System.Web;
using System.Web.Configuration;

namespace Lib.Web.Security
{
    public class LibAuthenticationModule : IHttpModule
    {
        private bool _isLibAuth = false;

        public event LibAuthenticationEventHandler OnAuthenticate;

        #region IHttpModule

        public void Init(HttpApplication context)
        {
            if (LibAuthenticationConfig.AuthRequired)
            {
                context.AuthenticateRequest += new EventHandler(context_AuthenticateRequest);
                context.EndRequest += new EventHandler(context_EndRequest);
            }
        }


        public void Dispose()
        {
            
        }

        #endregion

        #region Protected

        protected virtual void context_AuthenticateRequest(object sender, EventArgs e)
        {
            this._isLibAuth = true;

            HttpApplication application = sender as HttpApplication;
            if (application == null)
                return;

            HttpContext context = application.Context;
            if (context == null)
                return;

            if (this.Anonymous(context))
                return;

            if (LibAuthentication.AccessingLoginPage(context, LibAuthenticationConfig.LoginUrl))
                return;
            
            this.Authenticate(new LibAuthenticationEventArgs(context));
        }

        /// <summary>
        /// 允许匿名访问的资源先校验
        /// 如果需要校验，则允许匿名为False
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool Anonymous(HttpContext context)
        {
            bool needAuth = AuthenticateDirSettings.GetConfig().PageNeedAuthenticate(context.Request.Url.AbsoluteUri);
            if (needAuth)
                return false;
            return true;
        }

        protected virtual void context_EndRequest(object sender, EventArgs e)
        {
            if (!this._isLibAuth)
                return;

            this._isLibAuth = false;
        }

        protected virtual void Authenticate(LibAuthenticationEventArgs e)
        {
            if (this.OnAuthenticate != null)
                this.OnAuthenticate(this, e);

            if (e.Context.User == null || e.Context.User.Identity.AuthenticationType != LibAuthenticationConfig.AuthenticationType)
            {
                if (e.User != null)
                {
                    e.Context.User = e.User;
                }
                else
                {
                    bool fromCookie = true;
                    ILibAuthenticationTicket ticket = LibAuthentication.ExtractTicketFromCookie(ref fromCookie);
                    if (ticket == null)
                    {
                        LibAuthentication.RedirectLogin(e.Context);
                        return;
                    }
                    else
                    {
                        LibAuthentication.PrepareTicket(ticket);
                        e.Context.User = new LibPrincipal(new LibIdentity(ticket));
                        HttpCookie cookie = LibAuthentication.PrepareCookie(ticket, fromCookie);

                        e.Context.Response.Cookies.Remove(cookie.Name);
                        e.Context.Response.Cookies.Add(cookie);
                    }
                }
            }
        }

        #endregion
    }
}
