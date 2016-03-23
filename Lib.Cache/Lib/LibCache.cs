using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Cache
{
    public class LibCache : ObjectCache ,IEnumerable, IDisposable
    {
        private const DefaultCacheCapabilities c_capabilities = DefaultCacheCapabilities.InMemoryProvider
            | DefaultCacheCapabilities.CacheEntryChangeMonitors | DefaultCacheCapabilities.AbsoluteExpirations | DefaultCacheCapabilities.SlidingExpirations
            | DefaultCacheCapabilities.CacheEntryUpdateCallback | DefaultCacheCapabilities.CacheEntryRemovedCallback;

        private static object s_initLock = new object();
        private static LibCache s_cacheInstance;
        private static CacheEntryRemovedCallback s_removedCallback;
        private CacheStore[] _stores;
       

        public override object this[string key]
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override DefaultCacheCapabilities DefaultCacheCapabilities
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override System.Runtime.Caching.CacheItem AddOrGetExisting(System.Runtime.Caching.CacheItem value, System.Runtime.Caching.CacheItemPolicy policy)
        {
            throw new NotImplementedException();
        }

        public override object AddOrGetExisting(string key, object value, System.Runtime.Caching.CacheItemPolicy policy, string regionName = null)
        {
            throw new NotImplementedException();
        }

        public override object AddOrGetExisting(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null)
        {
            throw new NotImplementedException();
        }

        public override bool Contains(string key, string regionName = null)
        {
            throw new NotImplementedException();
        }

        public override CacheEntryChangeMonitor CreateCacheEntryChangeMonitor(IEnumerable<string> keys, string regionName = null)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public override object Get(string key, string regionName = null)
        {
            throw new NotImplementedException();
        }

        public override System.Runtime.Caching.CacheItem GetCacheItem(string key, string regionName = null)
        {
            throw new NotImplementedException();
        }

        public override long GetCount(string regionName = null)
        {
            throw new NotImplementedException();
        }

        public override IDictionary<string, object> GetValues(IEnumerable<string> keys, string regionName = null)
        {
            throw new NotImplementedException();
        }

        public override object Remove(string key, string regionName = null)
        {
            throw new NotImplementedException();
        }

        public override void Set(System.Runtime.Caching.CacheItem item, System.Runtime.Caching.CacheItemPolicy policy)
        {
            throw new NotImplementedException();
        }

        public override void Set(string key, object value, System.Runtime.Caching.CacheItemPolicy policy, string regionName = null)
        {
            throw new NotImplementedException();
        }

        public override void Set(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
