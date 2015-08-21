using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib.Core;

namespace Lib.Data
{
    public class AdapterFactory
    {
        public static object CreateAdapter(string key)
        {
            //添加cache
            object result = null;
            if (!DataAdapterCache.Instance.TryGetValue(key, out result))
            {
                result = AdapterConfigurationSection.GetConfig().Adapters[key].CreateInstance();
                DataAdapterCache.Instance.Add(key, result);
            }

            return result;
        }

        public static object CreateAdapter<T>()
        {
            System.Type type = typeof(T);

            if (AdapterConfigurationSection.GetConfig().Adapters.ContainsKey(type.Name))
                return CreateAdapter(type.Name);
            if (AdapterConfigurationSection.GetConfig().Adapters.ContainsKey(type.FullName))
                return CreateAdapter(type.FullName);

            throw new Exception(string.Format("无法找到类型：{0}对应的Adapter信息", type.FullName));
        }
    }
}
