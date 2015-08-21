using System;
using System.Text;
using Lib.Core;

namespace Lib.Data
{
    /// <summary>
    /// 提供一组字段和值的集合，帮助生成WHERE语句
    /// </summary>
    [Serializable]
    public class WhereSqlClauseBuilder : SqlClauseBuilderIUW, IConnectiveSqlClause
    {
        private LogicOperatorDefine logicOperator = LogicOperatorDefine.And;

        /// <summary>
        /// 构造方法
        /// </summary>
        public WhereSqlClauseBuilder()
            : base()
        {
        }

        /// <summary>
        /// 构造方法，可以指定生成条件表达式时的逻辑运算符
        /// </summary>
        /// <param name="lod">逻辑运算符</param>
        public WhereSqlClauseBuilder(LogicOperatorDefine lod)
            : base()
        {
            this.logicOperator = lod;
        }

        /// <summary>
        /// 条件表达式之间的逻辑运算符
        /// </summary>
        public LogicOperatorDefine LogicOperator
        {
            get
            {
                return this.logicOperator;
            }
            set
            {
                this.logicOperator = value;
            }
        }

        /// <summary>
        /// 判断是否不存在任何条件表达式
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return this.Count == 0;
            }
        }

        /// <summary>
        /// 帮助生成WHERE语句
        /// </summary>
        /// <param name="sqlBuilder">语句构造器</param>
        /// <returns>构造的Where子句(不含where部分)</returns>
        public override string ToSqlString(ISqlBuilder sqlBuilder)
        {
            ExceptionHelper.FalseThrow<ArgumentNullException>(sqlBuilder != null, "sqlBuilder");

            StringBuilder strB = new StringBuilder(256);

            foreach (SqlClauseBuilderItemIUW item in List)
            {
                if (strB.Length > 0)
                    strB.AppendFormat(" {0} ", EnumItemDescriptionAttribute.GetAttribute(this.logicOperator).ShortName);

                strB.Append(item.DataField);
                strB.AppendFormat(" {0} ", item.Operation);
                strB.Append(item.GetDataDesp(sqlBuilder));
            }
            return strB.ToString();
        }
    }
}
