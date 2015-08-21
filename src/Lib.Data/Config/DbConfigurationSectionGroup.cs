using Lib.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Lib.Data
{
    sealed class DbConfigurationSectionGroup : ConfigurationSectionGroup
    {
        public DbConfigurationSectionGroup()
            : base()
        {
        }

        [ConfigurationProperty("connectionManager")]
        public ConnectionManagerConfigurationSection ConnectionManager
        {
            get
            {
                return base.Sections["connectionManager"] as ConnectionManagerConfigurationSection;
            }
        }

        [ConfigurationProperty("transaction")]
        public TransactionConfigurationSection Transaction
        {
            get
            {
                return base.Sections["transaction"] as TransactionConfigurationSection;
            }
        }
    }
}
