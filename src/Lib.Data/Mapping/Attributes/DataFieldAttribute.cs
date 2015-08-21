using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lib.Data
{
    /// <summary>
    /// 列属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DataFieldAttribute : Attribute
    {
        public DataFieldAttribute()
        { }

        public DataFieldAttribute(string fieldName)
        {
            this.FieldName = fieldName;
        }

        public DataFieldAttribute(string fieldName, bool isNullable)
        {
            this.FieldName = fieldName;
            this.IsNullable = IsNullable;
        }

        /// <summary>
        /// 数据表列名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 是否标识列
        /// </summary>
        public bool IsIdentity { get; set; }


        /// <summary>
        /// 是否主键
        /// </summary>
        public bool IsPrimary { get; set; }

        /// <summary>
        /// 是否允许为空
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// 数据列长度
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefalutValue { get; set; }
    }
}
