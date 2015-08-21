using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using Lib.Core;
using Lib.Web.Mvc;

namespace Lib.Web
{
    public class ActionProviderFactory
    {
        private static ActionProvider CreateProvider(IHttpHandler handler, HttpRequest request)
        {
            if (handler is IUrlMappingHandler)
                return new UrlMappingActionProvider(handler, request);
            
            if(handler is LibController)
                return new  ControllerActionProvider(handler, request);

            if (handler is System.Web.UI.Page)
                return new ActionProvider(handler, request);

            return null;
        }

        public static void ExecuteProcess(IHttpHandler handler)
        {
            ExceptionHelper.FalseThrow<ArgumentNullException>(handler != null, "handler");

            ActionProvider actionProvider = ActionProviderFactory.CreateProvider(handler, HttpContext.Current.Request);
            actionProvider.ExecuteProcess();          
        }
    }
}
