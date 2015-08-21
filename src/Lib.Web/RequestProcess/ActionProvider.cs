using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Web;
using Lib.Core;
using Lib.Web.Mvc;

namespace Lib.Web
{
    public class ActionProvider
    {
        protected IHttpHandler Handler { get; set; }

        protected virtual String ActionKey
        {
            get { return "action"; }
        }

        protected Type HandlerType
        {
            get
            {
                return this.Handler.GetType();
            }
        }

        protected HttpRequest Request { get; set; }

        protected LibHandlerInfo HandlerInfo { get; set; }

        internal ActionProvider(IHttpHandler controller, HttpRequest request)
        {
            this.Handler = controller;
            this.Request = request;
        }

        internal void ExecuteProcess()
        {
            NameValueCollection parameters = null;
            MethodInfo mi = this.GetMatchMethod(out parameters);

            if (mi != null)
                mi.Invoke(this.Handler, PrepareActionParams(mi, parameters));
        }


        protected virtual MethodInfo GetMatchMethod(out NameValueCollection parameters)
        {
            ExceptionHelper.FalseThrow<ArgumentNullException>(this.HandlerType != null, "ControllerType");

            LibHandlerInfo handlerInfo = null;
            if (!LibHandlerInfoCache.Instance.TryGetValue(this.HandlerType, out handlerInfo))
            {
                handlerInfo = GetHandlerInfo();
                LibHandlerInfoCache.Instance.Add(this.HandlerType, handlerInfo);
            }
            this.HandlerInfo = handlerInfo;

            MethodInfo mi = GetMatchMethodByParams(out parameters);

            if (mi == null)
                mi = this.HandlerInfo.DefaultMethod;

            return mi;
        }

        protected virtual LibHandlerInfo GetHandlerInfo()
        {
            LibHandlerInfo controllers = GetControllerInstanceInfo();            

            MethodInfo[] mis = this.HandlerType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (MethodInfo mi in mis)
            {
                LibActionAttribute attr = AttributeHelper.GetCustomAttribute<LibActionAttribute>(mi);
                if (attr != null)
                {
                    controllers.Methods.Add(string.IsNullOrEmpty(attr.Name) ? mi.Name : attr.Name, mi);
                    if (controllers.DefaultMethod == null && attr.Default)
                        controllers.DefaultMethod = mi;
                }
            }
            return controllers;
        }

        protected virtual LibHandlerInfo GetControllerInstanceInfo()
        {
            return new LibHandlerInfo();
        }

        protected virtual MethodInfo GetMatchMethodByParams(out NameValueCollection requestParams)
        {
            string methodName = string.Empty;
            requestParams = GetRequestParameters(out methodName);

            Dictionary<string, MethodInfo> methodList = GetMatchedMethods(methodName);
            MethodInfo result = null;
            int maxMatchLevel = 0;

            foreach (KeyValuePair<string, MethodInfo> pair in methodList)
            {
                int level = GetParamsMatchedLevel(pair.Value, requestParams);
                if (level > maxMatchLevel)
                {
                    maxMatchLevel = level;
                    result = pair.Value;
                }
            }

            return result;
        }

        protected virtual Dictionary<string, MethodInfo> GetMatchedMethods(string methodName)
        {
            Dictionary<string, MethodInfo> methodList = new Dictionary<string, MethodInfo>();
            if (!string.IsNullOrEmpty(methodName))
            {
                if (this.HandlerInfo.Methods.ContainsKey(methodName))
                    methodList.Add(methodName, this.HandlerInfo.Methods[methodName]);

                ExceptionHelper.TrueThrow(methodList.Count == 0, "没有找到与Action：{0} 匹配的方法", methodName);
            }
            else
                methodList = this.HandlerInfo.Methods;

            return methodList;
        }

        protected virtual NameValueCollection GetRequestParameters(out string methodName)
        {
            NameValueCollection result = null;

            if (Request.RequestType.ToUpper() == "GET")
                result = new NameValueCollection(Request.QueryString);
            else
                result = new NameValueCollection(Request.Form);

            methodName = Request.QueryString[this.ActionKey];
            if (string.IsNullOrEmpty(methodName))
                methodName = Request.Form[this.ActionKey];

            result.Remove(ActionKey);

            return result;
        }

        protected virtual int GetParamsMatchedLevel(MethodInfo mi, NameValueCollection queryString)
        {
            int result = 0;
            ParameterInfo[] parameters = mi.GetParameters();

            if (parameters.Length == queryString.Count)
            {
                result++;
            }
            else
            {
                foreach (ParameterInfo param in parameters)
                {
                    if (queryString[param.Name] != null)
                        result++;
                }
                result -= Math.Abs(parameters.Length - result);	//方法参数的个数减去匹配程度，形成新的匹配度
            }
            return result;
        }

        protected virtual object[] PrepareActionParams(MethodInfo mi, NameValueCollection requestParams)
        {
            HttpRequest request = HttpContext.Current.Request;
            ParameterInfo[] parameters = mi.GetParameters();
            object[] paramValues = new object[parameters.Length];

            if (requestParams[this.ActionKey] != null)
                requestParams.Remove(this.ActionKey);

            if (parameters.Length == 1)
            {
                paramValues[0] = GetParameterValue(requestParams[0], parameters[0].ParameterType);
            }
            else
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    paramValues[i] = GetParameterValue(requestParams[parameters[i].Name], parameters[i].ParameterType);
                }
            }

            return paramValues;
        }

        protected virtual object GetParameterValue(string queryValue, Type type)
        {
            if (string.IsNullOrEmpty(queryValue))
                return null;
            else
                return DataConverter.ChangeType(queryValue, type);
        }
    }
}




