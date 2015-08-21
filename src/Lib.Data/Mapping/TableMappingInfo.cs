using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Lib.Data
{
    /// <summary>
    /// 实体与表之间的关系
    /// </summary>
    public class TableMappingInfo
    {
        public string TableName { get; set; }

        public string ClassName { get; set; }

        private FieldMappingInfoCollection fieldInfoCollection = null;
        public FieldMappingInfoCollection FieldInfoCollection
        {
            get
            {
                if (this.fieldInfoCollection == null)
                {
                    this.fieldInfoCollection = new FieldMappingInfoCollection();
                }
                return this.fieldInfoCollection;
            }
        }


        public static TableMappingInfo GetMappingInfo(System.Type type)
        {
            TableMappingInfo result = null;

            if (DataMappingCache.Instance.TryGetValue(type.FullName, out result))
                return result;
                        
            DataTableAttribute tableAttribute = DataTableAttribute.GetCustomAttribute(type, typeof(DataTableAttribute), true) as DataTableAttribute;
            if (tableAttribute == null)
                return null;

            result = new TableMappingInfo();
            result.TableName = tableAttribute.TableName;
            result.ClassName = type.Name;

            MemberInfo[] mis = GetTypeMembers(type);

            foreach (MemberInfo mi in mis)
            {
                if (mi.MemberType == MemberTypes.Field || mi.MemberType == MemberTypes.Property)
                {
                    FieldMappingInfo fieldMapping = CreateMappingItem(mi);
                    if (fieldMapping != null)
                    {
                        result.FieldInfoCollection.Add(fieldMapping);
                    }
                }
            }

            DataMappingCache.Instance.Add(type.FullName, result);

            return result;
        }

        private static FieldMappingInfo CreateMappingItem(MemberInfo mi)
        {
            FieldMappingInfo result = new FieldMappingInfo();

            DataFieldAttribute fieldAttribute = DataFieldAttribute.GetCustomAttribute(mi, typeof(DataFieldAttribute)) as DataFieldAttribute;
            if (fieldAttribute == null)
                return null;

            result.PropertyName = mi.Name;
            result.FieldName = string.IsNullOrEmpty(fieldAttribute.FieldName) ? mi.Name : fieldAttribute.FieldName;
            result.IsIdentity = fieldAttribute.IsIdentity;
            result.IsNullable = fieldAttribute.IsNullable;
            result.IsPrimary = fieldAttribute.IsPrimary;
            result.Length = fieldAttribute.Length;
            result.DefaultValue = fieldAttribute.DefalutValue;
                        
            result.MemberInfo = mi;

            return result;
        }

        private static MemberInfo[] GetTypeMembers(System.Type type)
        {
            List<MemberInfo> list = new List<MemberInfo>();

            PropertyInfo[] pis = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

            Array.ForEach(pis, delegate(PropertyInfo pi) { list.Add(pi); });

            FieldInfo[] fis = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);

            Array.ForEach(fis, delegate(FieldInfo fi) { list.Add(fi); });

            return list.ToArray();
        }
    }
}
