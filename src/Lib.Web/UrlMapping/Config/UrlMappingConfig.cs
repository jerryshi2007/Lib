using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib.Cache;

namespace Lib.Web
{
    public class UrlMappingConfig
    {
        public static bool IsIgnore(string key)
        {
            return UrlMappingSection.GetConfig().IgnoreItems.ContainsKey(key.ToLower());
        }

        public static void GetMappingItems()
        {
            foreach (UrlMappingElement element in UrlMappingSection.GetConfig().Pages)
            {
                UrlMappingItem item = GetMappingItemByElement(element, UrlMappingType.Page);
                if (item != null)
                {
                    if (!UrlMappingCache.Instance.ContainsKey(item.Key))
                        UrlMappingCache.Instance.Add(item.Key, item);
                }
            }

            foreach (UrlMappingElement element in UrlMappingSection.GetConfig().Handlers)
            {
                UrlMappingItem item = GetMappingItemByElement(element, UrlMappingType.Handler);
                if (item != null)
                {
                    if (!UrlMappingCache.Instance.ContainsKey(item.Key))
                        UrlMappingCache.Instance.Add(item.Key, item);
                }
            }
        }

        public static UrlMappingItem GetMappingItem(string key)
        {
            UrlMappingItem item = null;

            if (!UrlMappingCache.Instance.TryGetValue(key, out item))
            {
                GetMappingItems();
            }

            if (UrlMappingCache.Instance.TryGetValue(key, out item))
                return item;

            return null;
        }

        private static UrlMappingItem GetMappingItemByElement(UrlMappingElement element, UrlMappingType type)
        {
            UrlMappingItem item = new UrlMappingItem()
            {
                Key = element.Name,
                Type = type
            };

            switch (item.Type)
            {
                case UrlMappingType.Page:
                    item.Value = element.Url;
                    break;
                case UrlMappingType.Handler:
                    item.Value = element.Type;
                    break;
                default:
                    return null;
            }
            
            return item;
        }
    }

    public class UrlMappingCache : CacheQueue<string, UrlMappingItem>
    {
        public static readonly UrlMappingCache Instance = CacheManager.GetInstance<UrlMappingCache>();

        private UrlMappingCache()
        {
        }
    }
}
