using Lib.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Transactions;


namespace Lib.Data
{
    public delegate string ComposeSqlString<T>(T data);

    public abstract class DataAdapterBase
    {
        protected abstract string ConnectionName { get; }

        protected virtual string PageQueryString
        {
            get
            {
                return @"WITH TempQuery AS
                                    (
                                        SELECT {0}, ROW_NUMBER() OVER (ORDER BY {3}) AS 'RowNumberForSplit'
	                                    FROM {1}
	                                    WHERE 1 = 1 {2}
                                    )
                                    SELECT * 
                                    FROM TempQuery 
                                    WHERE RowNumberForSplit BETWEEN {4} AND {5};";
            }
        }

        public DataSet SplitPageQuery(QueryCondition qc, bool retrieveTotalCount)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(null == qc, "qc");
            ExceptionHelper.CheckStringIsNullOrEmpty(qc.SelectFields, "qc.SelectFields");
            ExceptionHelper.CheckStringIsNullOrEmpty(qc.FromClause, "qc.FromClause");
            ExceptionHelper.CheckStringIsNullOrEmpty(qc.OrderByClause, "qc.OrderByClause");

            DataSet ds = null;

            if (qc.RowIndex == 0 && qc.PageSize == 0)	//一种假设，qc.RowIndex == 0 && qc.PageSize == 0认为不分页
                ds = DoNoSplitPageQuery(qc);
            else
                ds = DoSplitPageQuery(qc, retrieveTotalCount);

            return ds;
        }

        private DataSet DoNoSplitPageQuery(QueryCondition qc)
        {
            string sql = string.Format("SELECT {0} FROM {1} WHERE 1 = 1 {2} ORDER BY {3}",
                        qc.SelectFields,
                        qc.FromClause,
                        string.IsNullOrEmpty(qc.WhereClause) ? string.Empty : " AND " + qc.WhereClause,
                        qc.OrderByClause);

            DataSet ds = DbHelper.ExecuteDataSet(ConnectionName, CommandType.Text, sql, new string[] { "RESULT" });

            DataTable table = new DataTable("RESULT_COUNT");

            table.Columns.Add("TOTAL_COUNT", typeof(int));
            table.Rows.Add(ds.Tables[0].Rows.Count);

            ds.Tables.Add(table);

            return ds;

        }

        private DataSet DoSplitPageQuery(QueryCondition qc, bool retrieveTotalCount)
        {
            string query = string.Format(
                this.PageQueryString,
                qc.SelectFields,
                qc.FromClause,
                string.IsNullOrEmpty(qc.WhereClause) ? string.Empty : " AND " + qc.WhereClause,
                qc.OrderByClause,
                qc.RowIndex + 1,
                qc.RowIndex + qc.PageSize);

            if (retrieveTotalCount)
                query += TSqlBuilder.Instance.DBStatementSeperator + GetTotalCountSql(qc);

                //根据SQL Server版本选择分页语句的写法
            return DbHelper.ExecuteDataSet(ConnectionName, CommandType.Text, query, new string[] { "RESULT", "RESULT_COUNT" });
 
        }


        private DataSet DoSplitPageQuery(QueryCondition qc, DbParameter[] dbParams, bool retrieveTotalCount)
        {
            string query = string.Format(
                this.PageQueryString,
                qc.SelectFields,
                qc.FromClause,
                string.IsNullOrEmpty(qc.WhereClause) ? string.Empty : " AND " + qc.WhereClause,
                qc.OrderByClause,
                qc.RowIndex + 1,
                qc.RowIndex + qc.PageSize);

            if (retrieveTotalCount)
                query += TSqlBuilder.Instance.DBStatementSeperator + GetTotalCountSql(qc);


                //根据SQL Server版本选择分页语句的写法
            return DbHelper.ExecuteDataSet(ConnectionName, CommandType.Text, query, new string[] { "RESULT", "RESULT_COUNT" });

        }

        private string GetTotalCountSql(QueryCondition qc)
        {
            return string.Format("SELECT COUNT(*) AS TOTAL_COUNT FROM {0} WHERE 1 = 1 {1}",
                        qc.FromClause,
                        string.IsNullOrEmpty(qc.WhereClause) ? string.Empty : " AND " + qc.WhereClause);
        }
    }
    ///<summary>
    ///带数据更新功能的DataAdapter的基类
    ///</summary>
    ///<typeparam name="T"></typeparam>
    public abstract class DataAdapterBase<T> : DataAdapterBase
    {
        #region Insert

        public virtual int Insert(T data)
        {
            return Insert(data, null);
        }

        public virtual int Insert(T data, ComposeSqlString<T> action)
        {
            return Insert(data, null, null);
        }

        public virtual int Insert(T data, ComposeSqlString<T> action, params DbParameter[] parameters)
        {
            if (data == null)
                throw new Exception("data 不能为空");

            Dictionary<string, object> context = new Dictionary<string, object>();

            int result = 0;

            BeforeInnerInsert(data, context);

            //using (TransactionScope scope = TransactionScopeFactory.Create())
            //{
            result = InnerInsert(data, action, parameters);
            if (result > 0)
                AfterInnerInsert(data, context);

            //   scope.Complete();
            //}

            return result;
        }

        protected virtual void BeforeInnerInsert(T data, Dictionary<string, object> context)
        {

        }

        protected virtual int InnerInsert(T data, ComposeSqlString<T> action, params DbParameter[] parameters)
        {
            string sql = GetSqlString(data, action, DataMapping.GetInsertText(data));

            return (int)DbHelper.ExecuteNonQuery(ConnectionName, CommandType.Text, sql, parameters);
        }

        protected virtual void AfterInnerInsert(T data, Dictionary<string, object> context)
        {

        }

        #endregion

        #region Update

        public virtual int Update(T data)
        {
            return Update(data, null);
        }

        public virtual int Update(T data, Dictionary<string,string> igonreProperty)
        {
            return Update(data, igonreProperty, null);
        }

        public virtual int Update(T data, Dictionary<string, string> igonreProperty, ComposeSqlString<T> action)
        {
            return Update(data,igonreProperty, null, null);
        }

        public virtual int Update(T data, Dictionary<string, string> igonreProperty, ComposeSqlString<T> action, params DbParameter[] parameters)
        {
            ExceptionHelper.FalseThrow<ArgumentNullException>(data != null, "data");

            int result = 0;

            Dictionary<string, object> context = new Dictionary<string, object>();
            context.Add("ignoreProperty", igonreProperty);

            BeforeInnerUpdate(data, context);

            //using (TransactionScope scope = TransactionScopeFactory.Create())
            //{
            result = InnerUpdate(data, context, action, parameters);
            if (result > 0)
                AfterInnerUpdate(data, context);

            //    scope.Complete();
            //}

            return result;
        }

        protected virtual void BeforeInnerUpdate(T data, Dictionary<string, object> context)
        {

        }

        protected virtual int InnerUpdate(T data, Dictionary<string, object> context, ComposeSqlString<T> action, params DbParameter[] parameters)
        {
            Dictionary<string, string> ignoreProperty = context["ignoreProperty"] as Dictionary<string, string>;

            string sql = GetSqlString(data, action, DataMapping.GetUpdateText(data, ignoreProperty));

            return (int)DbHelper.ExecuteNonQuery(ConnectionName, CommandType.Text, sql, parameters);
        }

        protected virtual void AfterInnerUpdate(T data, Dictionary<string, object> context)
        {

        }

        #endregion

        #region Delete

        public virtual int Delete(T data)
        {
            return Delete(data, null);
        }

        public virtual int Delete(T data, ComposeSqlString<T> action)
        {
            return Delete(data, null, null);
        }

        public virtual int Delete(T data, ComposeSqlString<T> action, params DbParameter[] parameters)
        {
            ExceptionHelper.FalseThrow<ArgumentNullException>(data != null, "data");

            int result = 0;

            Dictionary<string, object> context = new Dictionary<string, object>();

            BeforeInnerDelete(data, context);

            //using (TransactionScope scope = TransactionScopeFactory.Create())
            //{
            result = InnerDelete(data, action, parameters);

            if (result > 0)
                AfterInnerDelete(data, context);

            //    scope.Complete();
            //}

            return result;
        }

        protected virtual void BeforeInnerDelete(T data, Dictionary<string, object> context)
        {

        }

        protected virtual int InnerDelete(T data, ComposeSqlString<T> action, params DbParameter[] parameters)
        {
            string sql = GetSqlString(data, action, DataMapping.GetDeleteText(data));

            return (int)DbHelper.ExecuteNonQuery(ConnectionName, CommandType.Text, sql, parameters);
        }

        protected virtual void AfterInnerDelete(T data, Dictionary<string, object> context)
        {

        }

        #endregion

        #region ModelMapping

        protected virtual T DataModelMapping(DataRow dataSource, T data)
        {
            T result = data;

            DataMapping.DataRowToObject(dataSource, result);

            return result;

        }

        protected virtual T GetDataSourceValue(object dataSource, string fieldName, T defaultValue)
        {
            T result = defaultValue;

            if (dataSource is DataRow)
                result = (T)((DataRow)dataSource)[fieldName];
            else
                result = (T)((DbDataReader)dataSource)[fieldName];

            return result;
        }

        #endregion

        private string GetSqlString(T data, ComposeSqlString<T> action, string defaultString)
        {
            string sql = string.Empty;

            if (action != null)
                sql = action(data);

            if (string.IsNullOrEmpty(sql))
                sql = defaultString;

            return sql;
        }


    }
}
