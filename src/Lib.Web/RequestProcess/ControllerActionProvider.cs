using System;
using System.Collections.Specialized;
using System.Web;
using Lib.Core;

namespace Lib.Web.Mvc
{
    public class ControllerActionProvider : ActionProvider
    {
        protected LibController Controller
        {
            get
            {
                return this.Handler as LibController;
            }
        }

        protected override string ActionKey
        {
            get { return this.Controller.ActionKey; }
        }

        internal ControllerActionProvider(IHttpHandler handler, HttpRequest request)
            : base(handler, request)
        {
            ExceptionHelper.FalseThrow(handler is LibController, "请求的Handler类型不是LibController类型");
        }

        //protected override LibHandlerInfo GetControllerInstanceInfo()
        //{
        //    LibHandlerInfo result = new LibHandlerInfo();

        //    result.ActionKey = this.Controller.ActionKey;

        //    return result;
        //}

        //protected override NameValueCollection GetRequestParameters(out string methodName)
        //{
        //    NameValueCollection result = base.GetRequestParameters(out methodName);

        //    methodName = Request.QueryString[this.HandlerInfo.ActionKey];
        //    if (String.IsNullOrEmpty(methodName))
        //        methodName = Request.Form[this.HandlerInfo.ActionKey];

        //    result.Remove(this.HandlerInfo.ActionKey);

        //    return result;
        //}
  
    }
}
