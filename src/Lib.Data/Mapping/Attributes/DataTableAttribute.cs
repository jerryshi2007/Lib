using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lib.Data
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class DataTableAttribute : Attribute
    {
        public string TableName { get; set; }

        public DataTableAttribute(string tableName)
        {
            this.TableName = tableName;
        }
    }
}

