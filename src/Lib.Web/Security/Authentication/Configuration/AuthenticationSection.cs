using Lib.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Lib.Web.Security
{
    public class AuthenticationSectionGroup : ConfigurationSectionGroup
    {
        [ConfigurationProperty("authenticationServer")]
        public AuthenticationServerSection AuthenticationServer
        {
            get
            {
                return base.Sections["authenticationServer"] as AuthenticationServerSection;
            }
        }

        [ConfigurationProperty("authenticationClient")]
        public AuthenticationClientSection AuthenticationClient
        {
            get
            {
                return base.Sections["authenticationClient"] as AuthenticationClientSection;
            }
        }
    }
}
