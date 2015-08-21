using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace Lib.Web
{
    public interface IUrlMappingHandler : IHttpHandler
    {
        #region Field

        UrlMappingItem MappingItem
        {
            get;
            set;
        }

        NameValueCollection QueryString
        {
            get;
        }

        #endregion

   

    }
}
