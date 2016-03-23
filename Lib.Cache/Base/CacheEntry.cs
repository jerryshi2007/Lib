using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Cache
{
    /// <summary>
    /// Cache实体、缓存项
    /// 真正存储在CacheStore中的对象
    /// </summary>
    public class CacheEntry : CacheKey
    {
        public CacheEntry(string key, object value)
            : base(key)
        {
        }

        private object _value;

        private DateTime _utcCreateTime;
        private DateTime _utcAbsExpire;
        private TimeSpan _slidingExpire;
        private DateTime _utcUpdateTime;

        private CacheEntryRemovedCallback _removedCallback;

        public object Value
        {
            get { return _value; }
        }

        public DateTime UtcCreateTime
        {
            get { return _utcCreateTime; }
            set { _utcCreateTime = value; }
        }

        public DateTime UtcUpdateTime
        {
            get { return _utcUpdateTime; }
            set { _utcUpdateTime = value; }
        }

        public DateTime UtcAbsExpire
        {
            get { return _utcAbsExpire; }
            set { _utcAbsExpire = value; }
        }

        public bool IsExpiration
        {
            get { return _utcAbsExpire < DateTime.MaxValue; }
        }

        public TimeSpan SlidingExipre
        {
            get { return _slidingExpire; }
        }

        public CacheEntry(string key, object value, DateTimeOffset absExpire, TimeSpan slidingExpire, CacheItemPriority priority, Collection<ChangeMonitor> dependencies, CacheEntryRemovedCallback removedCallback, LibCache cache) : base(key)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            this._value = value;
            this._utcCreateTime = DateTime.UtcNow;
            this._slidingExpire = slidingExpire;

            if (this._slidingExpire > TimeSpan.Zero)
            {
                this._utcAbsExpire = this._utcCreateTime + this._slidingExpire;
            }
            else
            {
                this._utcAbsExpire = absExpire.UtcDateTime;
            }

            this._removedCallback = removedCallback;
        }
    }
}
