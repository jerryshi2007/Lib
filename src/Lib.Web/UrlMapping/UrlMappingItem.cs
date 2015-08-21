using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Lib.Core;

namespace Lib.Web
{
    public class UrlMappingItem
    {
        public string Key { get; set; }

        public UrlMappingType Type { get; set; }

        public string Value { get; set; }


        private List<string> _queryName = null;
        internal List<string> QueryName
        {
            get
            {
                if (this._queryName == null)
                    this._queryName = new List<string>();
                return this._queryName;
            }
        }

        private NameValueCollection _queryString = null;
        public NameValueCollection QueryString
        {
            get
            {
                if (this._queryString == null)
                    this._queryString = new NameValueCollection();
                return this._queryString;
            }
        }

        /// <summary>
        /// 建立对象的实例
        /// </summary>
        /// <param name="ctorParams">创建实例的初始化参数</param>
        /// <returns>运用晚绑定方式动态创建一个实例</returns>
        public object CreateHandler(params object[] ctorParams)
        {
            if (this.Type == UrlMappingType.Handler)
                return TypeCreator.CreateInstance(Value, ctorParams);
            return null;
        }
    }
}
