using System;

namespace Lib.Data
{
    /// <summary>
    /// 带数据的Sql语句构造项的基类
    /// </summary>
    [Serializable]
    public class SqlCaluseBuilderItemWithData : SqlClauseBuilderItemBase
    {
        private object data = null;
        private bool isExpression = false;

        /// <summary>
        /// 数据
        /// </summary>
        public object Data
        {
            get { return this.data; }
            set { this.data = value; }
        }

        /// <summary>
        /// 构想项中的Data是否是sql表达式
        /// </summary>
        public bool IsExpression
        {
            get { return this.isExpression; }
            set { this.isExpression = value; }
        }

        /// <summary>
        /// 得到Data的Sql字符串描述
        /// </summary>
        /// <param name="builder">构造器</param>
        /// <returns>返回将data翻译成sql语句的结果</returns>
        public override string GetDataDesp(ISqlBuilder builder)
        {
            string result = string.Empty;

            if (this.data == null || this.data is DBNull)
                result = "NULL";
            else
            {
                if (this.data is DateTime)
                {
                    if ((DateTime)this.data == DateTime.MinValue)
                        result = "NULL";
                    else
                        result = builder.FormatDateTime((DateTime)this.data);
                }
                else if (this.data is System.Guid)
                {
                    if ((Guid)this.data == Guid.Empty)
                        result = "NULL";
                    else
                        result = builder.CheckQuotationMark(this.data.ToString(), true);
                }
                else
                {
                    if (this.isExpression == false && (this.data is string || this.data.GetType().IsEnum))
                        result = builder.CheckQuotationMark(this.data.ToString(), true);
                    else
                        if (this.data is bool)
                            result = ((int)Convert.ChangeType(this.data, typeof(int))).ToString();
                        else
                            result = this.data.ToString();
                }
            }

            return result;
        }
    }
}
