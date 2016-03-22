using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Cache
{
    public class LibMemoryCacheEntryChangeMonitor : CacheEntryChangeMonitor
    {
        public override ReadOnlyCollection<string> CacheKeys
        {
            get { throw new NotImplementedException(); }
        }

        public override DateTimeOffset LastModified
        {
            get { throw new NotImplementedException(); }
        }

        public override string RegionName
        {
            get { throw new NotImplementedException(); }
        }

        protected override void Dispose(bool disposing)
        {
            throw new NotImplementedException();
        }

        public override string UniqueId
        {
            get { throw new NotImplementedException(); }
        }
    }
}
