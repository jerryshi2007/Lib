using System;
using System.Collections.Generic;
using System.Web;
using Lib.Web.Mvc;

namespace Lib.Web
{
    public class HtmlResponseWriter : ResponseWriter
    {
        public HtmlResponseWriter(LibViewModel result)
            : base(result)
        { }

        protected override void BeforeWriteResponse(HttpResponse response)
        {
            base.BeforeWriteResponse(response);

            response.ContentType = "text/html";
        }

        protected override void InnerWriteResponse(System.Web.HttpResponse response)
        {
            response.Write(this.ViewModel.HtmlString);
        }
    }
}
