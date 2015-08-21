using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Lib.Data
{
    /// <summary>
    /// 描述对象属性与表列之间的关系
    /// </summary>
    public class FieldMappingInfo
    {
        /// <summary>
        /// 数据表列名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 实体属性名
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 是否允许为空
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// 数据表列长度
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 数据表列是否为标识，自增字段
        /// </summary>
        public bool IsIdentity { get; set; }

        /// <summary>
        /// 是否主键
        /// </summary>
        public bool IsPrimary { get; set; }

        /// <summary>
        /// 默认值，可以是数值，也可以是SQL 函数，如NEWID() , GETDATE()
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// 实体类属性的成员信息
        /// </summary>
        public MemberInfo MemberInfo { get; set; }


    }

    public class FieldMappingInfoCollection : KeyedCollection<string, FieldMappingInfo>
    {
        protected override string GetKeyForItem(FieldMappingInfo item)
        {
            return item.FieldName;
        }

        public virtual string ToFieldString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, FieldMappingInfo> pair in this.Dictionary)
            {
                builder.AppendFormat("{0},", pair.Key);
            }

            return builder.Remove(builder.Length - 1, 1).ToString();
        }
    }
}
