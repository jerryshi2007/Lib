using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace Lib.Caching
{
    public abstract class LibCache<T> : MemoryCache
    {
        public LibCache(string name, NameValueCollection config = null)
            : base(name, config)
        { }


        //protected virtual string RegionName
        //{
        //    get
        //    {
        //        return this.GetType().FullName;
        //    }
        //}

        public bool TryGetValue(string key, out T data)
        {
            data = default(T);
            
            bool result = false;

            CacheItem item = this.GetCacheItem(key);
            if (item != null)
            {
                data = (T)item.Value;
                if (data != null)
                    result = true;
            }
            return result;
        }



        public void Add(string key, T value)
        {
            CacheItem item = new CacheItem(key, value);
            CacheItemPolicy policy = new CacheItemPolicy();
            
            this.Add(item, policy);
        }
    }
}
