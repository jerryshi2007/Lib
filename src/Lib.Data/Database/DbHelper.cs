using Lib.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Lib.Data
{
    public static class DbHelper
    {
        public static object ExecuteScalar(string name, CommandType commandType, string sql, params DbParameter[] dbParams)
        {
            using (DbContext context = DbContext.GetContext(name))
            {
                Database db = DatabaseFactory.Create(context);

                return db.ExecuteScalar(commandType, sql, dbParams);
            }
        }

        public static int ExecuteNonQuery(string name, CommandType commandType, string sql, params DbParameter[] dbParams)
        {
            using (DbContext context = DbContext.GetContext(name))
            {
                Database db = DatabaseFactory.Create(context);

                return db.ExecuteNonQuery(commandType, sql, dbParams);
            }
        }

        public static DbDataReader ExecuteReader(DbContext context, CommandType commandType, string sql, params DbParameter[] dbParams)
        {
            Database db = DatabaseFactory.Create(context);

            DbDataReader reader = db.ExecuteReader(commandType, sql, dbParams);

            return reader;
        }

        public static DataSet ExecuteDataSet(string name, CommandType commandType, string sql, params DbParameter[] dbParams)
        {
            using (DbContext context = DbContext.GetContext(name))
            {
                Database db = DatabaseFactory.Create(context);

                return db.ExecuteDataSet(commandType, sql, null, dbParams);
            }
        }

        public static DataSet ExecuteDataSet(string name, CommandType commandType, string sql, string[] tableNames, params DbParameter[] dbParams)
        {
            using (DbContext context = DbContext.GetContext(name))
            {
                Database db = DatabaseFactory.Create(context);

                return db.ExecuteDataSet(commandType, sql, tableNames, dbParams);
            }
        }

    }
}
