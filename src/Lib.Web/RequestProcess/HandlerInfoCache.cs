using System;
using System.Collections.Generic;
using Lib.Cache;

namespace Lib.Web
{
    public class LibHandlerInfoCache : CacheQueue<System.Type, LibHandlerInfo>
    {
        public static readonly LibHandlerInfoCache Instance = CacheManager.GetInstance<LibHandlerInfoCache>();

        private LibHandlerInfoCache()
        {
        }
    }
}
