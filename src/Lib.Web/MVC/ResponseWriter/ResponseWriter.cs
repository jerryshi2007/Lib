using System;
using System.Collections.Generic;
using System.Web;
using Lib.Web.Json;
using Lib.Web.Mvc;

namespace Lib.Web
{
    public abstract class ResponseWriter
    {
        protected LibViewModel ViewModel { get; set; }

        public ResponseWriter(LibViewModel model)
        {
            this.ViewModel = model;
        }

        public static void Write(LibViewModel model)
        {
            ResponseWriter writer = CreateResponseWriter(model);
            writer.Write();
        }

        public static void Write(LibViewModel model, Action<HttpResponse> action)
        {
            ResponseWriter writer = CreateResponseWriter(model);
            writer.Write(action);
        }
        
        public static void Write(string content)
        {
            HttpContext.Current.Response.Write(content);
        }

        public static ResponseWriter CreateResponseWriter(LibViewModel viewModel)
        {
            switch (viewModel.ModeType)
            { 
                case LibViewModelType.Json:
                    return new JsonResponseWriter(viewModel);
                case LibViewModelType.Html:
                    return new HtmlResponseWriter(viewModel);
                default :
                    return new JsonResponseWriter(viewModel);
            }
        }

        public void Write()
        {
            HttpResponse response = PrepareResponse();

            BeforeWriteResponse(response);

            InnerWriteResponse(response);

            AfterWriteResponse(response);
        }

        public void Write(Action<HttpResponse> action)
        {
            HttpResponse response = PrepareResponse();

            BeforeWriteResponse(response);

            InnerWriteResponse(response);

            AfterWriteResponse(response);
        }

        protected virtual void AfterWriteResponse(HttpResponse response)
        {
            
        }

        protected abstract void InnerWriteResponse(HttpResponse response);

        protected virtual HttpResponse PrepareResponse()
        {
            HttpContext.Current.Response.Cache.SetNoStore();

            return HttpContext.Current.Response;
        }

        protected virtual void BeforeWriteResponse(HttpResponse response)
        { }
    }
}
