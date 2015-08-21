using Lib.Caching;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace Lib.Data
{
    public class DataMappingCache : LibCache<TableMappingInfo>
    {

        public DataMappingCache(string name, NameValueCollection config = null)
            : base(name, config)
        { }

        private static DataMappingCache _instance = null;
        public static DataMappingCache Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DataMappingCache("DataMappingCache");
                return _instance;
            }
        }




   
    }
}
