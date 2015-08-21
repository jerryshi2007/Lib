using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Compilation;

namespace Lib.Web
{
    public class UrlMappingHelper
    {
        internal static IHttpHandler GetHandler(HttpContext context)
        {
            IHttpHandler handler = null;
            NameValueCollection queryString = null;

            UrlMappingItem umItem = AnalyseRequest(context, out queryString);

            handler = GetHandler(context, umItem, queryString);

            return handler;
        }

        private static UrlMappingItem AnalyseRequest(HttpContext context, out NameValueCollection queryString)
        {
            string rawUrl = context.Request.RawUrl;
            queryString = null;

            if (UrlMappingConfig.IsIgnore(context.Request.CurrentExecutionFilePathExtension))
                return null;

            UrlMappingItem item = AnalyseRequestRawUrl(rawUrl, out queryString);

            return item;
        }

        private static UrlMappingItem AnalyseRequestRawUrl(string rawUrl, out NameValueCollection queryString)
        {
            string key = string.Empty;

            key = rawUrl.TrimStart('/').TrimEnd('/');

            List<string> parameters = new List<string>(key.Split('/'));
            if (string.IsNullOrWhiteSpace(parameters[0]))
                parameters.RemoveAt(0);

            return AnalyseRequestRawUrl(parameters, 0, out queryString);
        }

        private static UrlMappingItem AnalyseRequestRawUrl(List<string> urlParams, int level, out NameValueCollection queryString)
        {
            UrlMappingItem item = null;
            queryString = null;
            if (level >= urlParams.Count)
                return null;

            int length = urlParams.Count - level;
            string key = string.Empty;
            for (int i = 0; i < length; i++)
            {
                key += urlParams[i];
                if (i < length - 1)
                {
                    key += "/";
                }
            }

            item = UrlMappingConfig.GetMappingItem(key);
            if (item != null)
            {
                queryString = PrepareQueryString(item, urlParams, level);
                return item;
            }
            else
            {
                level = level + 1;
                return AnalyseRequestRawUrl(urlParams, level, out queryString);
            }
        }

        private static NameValueCollection PrepareQueryString(UrlMappingItem item, List<string> urlParams, int level)
        {
            if (urlParams.Count <= level)
                return null;
            if (item.QueryName.Count == 0)
                return null;

            NameValueCollection queryString = new NameValueCollection();
            for (int index = urlParams.Count - level, nameIndex = 0;
                    index < urlParams.Count && nameIndex < item.QueryName.Count;
                    index++, nameIndex++)
            {
                queryString.Add(item.QueryName[nameIndex], urlParams[index]);
            }

            return queryString;
        }

        private static IHttpHandler GetHandler(HttpContext context, UrlMappingItem mappingItem, NameValueCollection queryString)
        {
            if (mappingItem == null)
                return null;

            IHttpHandlerFactory factory = new UrlMappingHandlerFactory(mappingItem, queryString);

            return factory.GetHandler(context, context.Request.RequestType, mappingItem.Value, context.Request.PhysicalApplicationPath);
        }
    }
}
