using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Lib.Web.Mvc;

namespace Lib.Web
{
    public abstract class LibUrlMappingController : LibController, IUrlMappingHandler
    {
        public UrlMappingItem MappingItem
        {
            get;
            set;
        }

        private NameValueCollection _querystring = null;
        public NameValueCollection QueryString
        {
            get
            {
                if (this._querystring == null)
                    this._querystring = new NameValueCollection();
                return this._querystring;
            }
        }
    }
}
