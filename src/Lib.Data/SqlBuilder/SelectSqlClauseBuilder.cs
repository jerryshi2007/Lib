using System;
using Lib.Core;
using System.Text;

namespace Lib.Data
{
    public class SelectSqlClauseBuilder : SqlClauseBuilderBase
    {
        public void AppendItem(string fieldName)
        {
            SelectSqlClauseBuilderItem item = new SelectSqlClauseBuilderItem();
            item.DataField = fieldName;

            List.Add(item);
        }

        public override string ToSqlString(ISqlBuilder sqlBuilder)
        {
            ExceptionHelper.TrueThrow(sqlBuilder == null, "{0} 不能为空", sqlBuilder);

            StringBuilder builder = new StringBuilder();

            foreach (SelectSqlClauseBuilderItem item in List)
            {
                if (builder.Length > 0)
                    builder.Append(",");

                builder.Append(item.DataField);

                string sqlValue = item.GetDataDesp(sqlBuilder);
                if (!string.IsNullOrEmpty(sqlValue))
                {
                    builder.Append(" ");
                    builder.Append(sqlValue);
                }
            }

            return builder.ToString();
        }
    }
}
