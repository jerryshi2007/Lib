using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using Lib.Web.Mvc;

namespace Lib.Web
{
    public class UrlMappingActionProvider : ControllerActionProvider
    {
        private bool _isIgnore = true;

        internal UrlMappingActionProvider(IHttpHandler controller, HttpRequest request)
            : base(controller, request)
        {
            this._isIgnore = UrlMappingConfig.IsIgnore(request.CurrentExecutionFilePathExtension);
        }

        //protected override NameValueCollection GetRequestParameters(out string methodName)
        //{
        //    IUrlMappingHandler handler = this.Handler as IUrlMappingHandler;
        //    if (handler == null || this._isIgnore)
        //        return base.GetRequestParameters(out methodName);

        //    methodName = handler.QueryString["0"];
        //    NameValueCollection result = null;

        //    if (string.Compare("get", this.Request.HttpMethod.ToLower()) == 0)
        //    {
        //        result = new NameValueCollection(handler.QueryString);
        //        result.Remove("0");
        //    }
        //    else
        //    {
        //        result = new NameValueCollection(this.Request.Form);
        //        if (string.IsNullOrEmpty(methodName))
        //            methodName = result[this.HandlerInfo.ActionKey];
        //        result.Remove(this.HandlerInfo.ActionKey);
        //    }
        //    return result;
        //}

    }
}
