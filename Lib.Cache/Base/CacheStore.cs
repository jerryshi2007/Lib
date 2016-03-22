using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Cache
{
    /// <summary>
    /// 缓存存储类，Store负责真正的读取和存储数据
    /// 默认为内存存储，可扩展使用MemoryCached 或Redis及其他内存数据库
    /// </summary>
    public abstract class CacheStore : IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
