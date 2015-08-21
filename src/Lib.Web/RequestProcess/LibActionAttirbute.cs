using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lib.Web
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class LibActionAttribute : Attribute
    {
        private bool _isDefault = false;
        private string _name = string.Empty;

        /// <summary>
        /// 构造方法
        /// </summary>
        public LibActionAttribute()
        {
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="isdefault"></param>
        public LibActionAttribute(bool isdefault)
        {
            this._isDefault = isdefault;
        }

        public LibActionAttribute(string name)
        {
            this._name = name;
        }

        public LibActionAttribute(bool isdefault, string name)
        {
            this._isDefault = isdefault;
            this._name = name;
        }

        /// <summary>
        /// 是否是缺省的方法
        /// </summary>
        public bool Default
        {
            get { return this._isDefault; }
            set { this._isDefault = value; }
        }


        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }
    }
}
