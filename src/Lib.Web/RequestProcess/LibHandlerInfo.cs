using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Lib.Web
{
    public class LibHandlerInfo
    {
        private MethodInfo defaultMethod = null;

        private Dictionary<string, MethodInfo> methods = null;

        public LibHandlerInfo()
        {
        }

        public Dictionary<string, MethodInfo> Methods
        {
            get
            {
                if (this.methods == null)
                    this.methods = new Dictionary<string, MethodInfo>();
                return this.methods;
            }
        }

        public MethodInfo DefaultMethod
        {
            get
            {
                return defaultMethod;
            }
            set
            {
                this.defaultMethod = value;
            }
        }

        //public string ActionKey { get; set; }

    }
}
