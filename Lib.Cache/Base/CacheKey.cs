using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Cache
{
    /// <summary>
    /// 
    /// </summary>
    internal class CacheKey
    {
        internal CacheKey(String key)
        {
            _key = key;
            _hash = key.GetHashCode();
        }         

        private String _key;
        private int _hash;
        protected byte _bits;

        internal int Hash
        {
            get { return _hash; }
        }
        internal String Key
        {
            get { return _key; }
        }

         
    }
}
