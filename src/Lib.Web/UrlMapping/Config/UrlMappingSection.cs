using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Lib.Config;

namespace Lib.Web
{
    public class UrlMappingSection : ConfigurationSection
    {
        public static UrlMappingSection GetConfig()
        {
            UrlMappingSection section =
                (UrlMappingSection)ConfigurationBroker.GetSection("urlMapping");

            ConfigurationExceptionHelper.CheckSectionNotNull(section, "urlMapping");

            return section;
        }

        [ConfigurationProperty("ignore")]
        public UrlMappingElementCollection IgnoreItems
        {
            get
            {
                return (UrlMappingElementCollection)this["ignore"];
            }
        }

        [ConfigurationProperty("pages")]
        public UrlMappingElementCollection Pages
        {
            get
            {
                return (UrlMappingElementCollection)this["pages"];
            }
        }

        [ConfigurationProperty("handlers")]
        public UrlMappingElementCollection Handlers
        {
            get
            {
                return (UrlMappingElementCollection)this["handlers"];
            }
        }
    }
}
