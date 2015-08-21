using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Lib.Data
{
    public abstract class DbContext : IDisposable
    {
        private bool autoClose = true;

        /// <summary>
        /// 获取一个DbContext对象
        /// </summary>
        /// <param name="name">连接名称</param>
        /// <param name="autoClose">是否自动关闭</param>
        /// <returns>DbContext对象</returns>
        public static DbContext GetContext(string name, bool autoClose)
        {
            //得到映射后连接名称，该映射是为了高可用方案作的数据库连接映射
            //一般情况下不需要
            //name = DbConnectionMappingContext.GetMappedConnectionName(name);

            DbProviderFactory factory = DbConnectionManager.GetDbProviderFactory(name);

            DbConnectionStringBuilder csb = factory.CreateConnectionStringBuilder();

            csb.ConnectionString = DbConnectionManager.GetConnectionString(name);

            bool enlist = true;

            if (csb.ContainsKey("enlist"))
                enlist = (bool)csb["enlist"];

            DbContext result = null;

            if (enlist)
                result = new AutoEnlistDbContext(name, autoClose);
            else
                result = new NotEnlistDbContext(name, autoClose);

            result.autoClose = autoClose;

            return result;
        }

        /// <summary>
        /// 重载获取DbContext对象
        /// </summary>
        /// <param name="name">连接名称</param>
        /// <returns></returns>
        public static DbContext GetContext(string name)
        {
            return GetContext(name, true);
        }

        #region Public 成员

        /// <summary>
        /// 是否自动关闭
        /// </summary>
        public bool AutoClose
        {
            get
            {
                return this.autoClose;
            }
            private set
            {
                this.autoClose = value;
            }
        }

        /// <summary>
        /// 数据连接对象
        /// </summary>
        public abstract DbConnection Connection
        {
            get;
            internal protected set;
        }

        /// <summary>
        /// 数据事务对象
        /// </summary>
        public abstract DbTransaction LocalTransaction
        {
            get;
            internal protected set;
        }

        /// <summary>
        /// 数据连接名称
        /// </summary>
        public abstract string Name
        {
            get;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
        }

        #endregion
    }
}
