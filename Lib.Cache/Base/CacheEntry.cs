using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Cache
{
    /// <summary>
    /// Cache实体、缓存项
    /// 真正存储在CacheStore中的对象
    /// </summary>
    internal class CacheEntry : CacheKey
    {
        internal CacheEntry(string key, object value)
            : base(key)
        {
        }

        private object _value;

        private DateTime _utcCreateTime;
        private DateTime _utcAbsExpire;
        private TimeSpan _slidingExpire;



    }
}
