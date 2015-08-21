using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Lib.Config;

namespace Lib.Data
{
    public class AdapterElement : TypeConfigurationElement
    {

    }

    public class AdapterElementCollection : NamedConfigurationElementCollection<AdapterElement>
    { 
    
    }
}
