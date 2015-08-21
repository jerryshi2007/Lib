using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Lib.Web
{
    public class UrlMappingHandlerFactory : IHttpHandlerFactory
    {
        private UrlMappingItem _item = null;
        private NameValueCollection _query = null;

        internal UrlMappingHandlerFactory(UrlMappingItem item, NameValueCollection queryString)
        {
            this._item = item;
            this._query = queryString;
        }

        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            if (this._item.Type == UrlMappingType.Page)
            {
                PageHandlerFactory pageFactory = Activator.CreateInstance(typeof(PageHandlerFactory), true) as PageHandlerFactory;
                if (pageFactory != null)
                    return pageFactory.GetHandler(context, requestType, url, pathTranslated);
            }

            if (this._item.Type == UrlMappingType.Handler)
            {
                return InnerGetHandler(context);
            }

            return null;
        }

        public void ReleaseHandler(IHttpHandler handler)
        {
            
        }

        private IHttpHandler InnerGetHandler(HttpContext context)
        {
            IHttpHandler handler = null;
            try
            {
                handler = this._item.CreateHandler() as IHttpHandler;
                if (handler is IUrlMappingHandler)
                {
                    IUrlMappingHandler umHandler = handler as IUrlMappingHandler;
                    umHandler.MappingItem = this._item;
                    umHandler.QueryString.Add(this._query);
                    return umHandler;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return handler;
        }
    }
}
