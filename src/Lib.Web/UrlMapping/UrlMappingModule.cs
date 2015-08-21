using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Lib.Web
{
    public class UrlMappingModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.PostResolveRequestCache += context_PostResolveRequestCache;
        }

        void context_PostResolveRequestCache(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;
            if (app == null)
                throw new Exception("UrlMappingModule：无效的请求上线文");
            
            IHttpHandler handler = UrlMappingHelper.GetHandler(app.Context);
            if (handler != null)
                app.Context.RemapHandler(handler);
        }

        public void Dispose()
        {
        }
    }
}
