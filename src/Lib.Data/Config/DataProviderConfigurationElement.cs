using Lib.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Lib.Data
{
    sealed class DataProviderConfigurationElement : TypeConfigurationElement
    {

    }

    [ConfigurationCollection(typeof(DataProviderConfigurationElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    sealed class DataProviderConfigurationElementCollection : NamedConfigurationElementCollection<DataProviderConfigurationElement>
    {

    }
}
