using System;
using Lib.Cache;


namespace Lib.Data
{
    public class DataAdapterCache : CacheQueue<string, object>
    {
        public static readonly DataAdapterCache Instance = CacheManager.GetInstance<DataAdapterCache>();

        private DataAdapterCache()
        {
        }
    }
}
