using System;
using System.Collections.Specialized;
using System.Web;
using Lib.Web;

namespace Lib.Web.Mvc
{
    public abstract class LibController : IHttpHandler
    {
        public virtual bool IsReusable
        {
            get { return false; }
        }

        public virtual string ActionKey
        {
            get { return "action"; }
        }

        public void ProcessRequest(HttpContext context)
        {
            WebUtility.CheckHttpContext();

            BeforeProcess(context);

            WebUtility.ExecuteProcess(this);

            AfterProcess(context);
        }

        protected virtual void AfterProcess(HttpContext context)
        {

        }

        protected virtual void BeforeProcess(HttpContext context)
        {

        }
    }

   
}
