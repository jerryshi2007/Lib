using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Lib.Config;

namespace Lib.Web
{
    public class UrlMappingElement : NamedConfigurationElement
    {
        [ConfigurationProperty("url")]
        public string Url
        {
            get
            {
                return (string)base["url"];
            }
        }

        /// <summary>
        /// 类型描述信息
        /// </summary>
        /// <remarks>一般采用QualifiedName （QuanlifiedTypeName, AssemblyName）方式</remarks>
        [ConfigurationProperty("type")]
        public string Type
        {
            get
            {
                return (string)this["type"];
            }
        }
    }

    public class UrlMappingElementCollection : NamedConfigurationElementCollection<UrlMappingElement>
    {

    }
}
