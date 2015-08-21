using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Lib.Config;

namespace Lib.Data
{
    public class AdapterConfigurationSection : ConfigurationSection
    {
        public static AdapterConfigurationSection GetConfig()
        {
            AdapterConfigurationSection section =
                (AdapterConfigurationSection)ConfigurationBroker.GetSection("adapterSettings");

            ConfigurationExceptionHelper.CheckSectionNotNull(section, "adapterSettings");

            return section;
        }

        [ConfigurationProperty("adapters")]
        public AdapterElementCollection Adapters
        {
            get
            {
                return (AdapterElementCollection)this["adapters"];
            }
        }
    }
}
