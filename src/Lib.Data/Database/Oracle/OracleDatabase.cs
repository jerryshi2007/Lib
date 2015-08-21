using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using Lib.Data.Properties;
using System.Data;
using Lib.Core;

namespace Lib.Data
{
    /// <summary>
    /// Oracle数据库对象
    /// </summary>
    /// <remarks>
    /// 使用上需要注意如下内容：
    /// <list>
    ///     <item>
    ///     所有Oracle存储过程的名称需采用Qualified Name， 即[SchemaName].[PackageName].[Procedure/Function]的方式，
    ///     根据调用上下文，可以省略[SchemaName]， 但如果调用的是Package中的Proceduere/Function时，[PackageName]部分不可省略
    ///     </item>
    ///     <item>
    ///     如果准备通过Oracle存储过程返回查询结果，需要在存储过程中将其定义为REF CURSOR或等价的Type
    ///     </item>
    /// </list>
    /// </remarks>
    public class OracleDatabase : Database
    {
        #region Internal Type OracleParameterTypeRegistry
        // 出于随即调用和效率考虑,这里增加了一个本地的注册类
        // 它被sealed, 而且在进行IDcitionary操作的时候不采用TryGetValue方式，而是直接的Indexer方式。
        internal sealed class ParameterTypeRegistry
        {
            private IDictionary<string, DbType> dictionary = new Dictionary<string, DbType>();

            internal void Registry(string parameterName, DbType parameterType)
            {
                dictionary[parameterName] = parameterType;
            }

            //internal bool IsRegistered(string parameterName)
            //{
            //    return dictionary.ContainsKey(parameterName);
            //}

            //internal DbType Get(string parameterName)
            //{
            //    return dictionary[parameterName];
            //}
        }

        #endregion

        #region Private Fields and Consts
        private const string DeluxeWorksOracleCursorNameRoot = "dworks_cur_out";
        private const string OracleDefaultReturnValueName = "RETURN_VALUE";
        private IDictionary<string, ParameterTypeRegistry> parameterTypesRegistry = new Dictionary<string, ParameterTypeRegistry>();

        //private static DbParameterCache cache = new DbParameterCache(); // 线程安全的
        #endregion

        #region Constructor
        /// <summary>
        /// 通过逻辑名称构造数据库对象实例
        /// </summary>
        /// <param name="name">逻辑名称</param>
        public OracleDatabase(string name)
            : base(name)
        {
            this.factory = OracleClientFactory.Instance;
        }
        #endregion

        #region Parameter Mechanism
        /// <summary>
        /// 对于存储过程（尤其是Function）返回结果的参数名称
        /// </summary>
        protected override string DefaultReturnValueParameterName
        {
            get
            {
                return "RETURN_VALUE";
            }
        }

        /// <summary>
        /// 为Command添加Parameter
        /// </summary>
        /// <param name="command">Command对象</param>
        /// <param name="dbType">操作类型</param>
        /// <param name="direction">参数类型</param>
        /// <param name="name">名称</param>
        /// <param name="nullable">是否可空</param>
        /// <param name="precision">参数精度</param>
        /// <param name="scale">大小</param>
        /// <param name="size">长度</param>
        /// <param name="sourceColumn">数据项名称</param>
        /// <param name="sourceVersion">DataRow版本</param>
        /// <param name="value">具体参数值</param>
        public override void AddParameter(DbCommand command,
            string name,
            DbType dbType,
            int size,
            ParameterDirection direction,
            bool nullable,
            byte precision,
            byte scale,
            string sourceColumn,
            DataRowVersion sourceVersion,
            object value)
        {
            if (DbType.Guid.Equals(dbType))
            {
                object convertedValue = ConvertGuidToByteArray(value);

                AddParameter((OracleCommand)command,
                    name,
                    OracleType.Raw,
                    16,
                    direction,
                    nullable,
                    precision,
                    scale,
                    sourceColumn,
                    sourceVersion,
                    convertedValue);

                RegisterParameterType(command, name, dbType);
            }
            else
            {
                base.AddParameter(command,
                    name,
                    dbType,
                    size,
                    direction,
                    nullable,
                    precision,
                    scale,
                    sourceColumn,
                    sourceVersion,
                    value);
            }
        }

        /// <summary>
        /// 为Command添加Parameter
        /// </summary>
        /// <param name="command">Command对象</param>
        /// <param name="oracleType">操作类型</param>
        /// <param name="direction">参数类型</param>
        /// <param name="name">名称</param>
        /// <param name="nullable">是否可空</param>
        /// <param name="precision">参数精度</param>
        /// <param name="scale">大小</param>
        /// <param name="size">长度</param>
        /// <param name="sourceColumn">数据项名称</param>
        /// <param name="sourceVersion">DataRow版本</param>
        /// <param name="value">值</param>
        public void AddParameter(OracleCommand command,
            string name,
            OracleType oracleType,
            int size,
            ParameterDirection direction,
            bool nullable,
            byte precision,
            byte scale,
            string sourceColumn,
            DataRowVersion sourceVersion,
            object value)
        {
            OracleParameter param = CreateParameter(name,
                DbType.AnsiString,
                size,
                direction,
                nullable,
                precision,
                scale,
                sourceColumn,
                sourceVersion,
                value) as OracleParameter;

            param.OracleType = oracleType;
            command.Parameters.Add(param);
        }

        /// <summary>
        /// 为Command添加Parameter
        /// </summary>
        /// <param name="command">Command对象</param>
        /// <param name="oracleType">操作类型</param>
        /// <param name="direction">参数类型</param>
        /// <param name="name">名称</param>
        /// <param name="size">长度</param>
        /// <param name="sourceColumn">数据项名称</param>
        public void AddParameter(OracleCommand command,
            string name,
            OracleType oracleType,
            int size,
            ParameterDirection direction,
            string sourceColumn)
        {
            OracleParameter param = CreateParameter(name, DbType.AnsiString, size, direction, sourceColumn) as OracleParameter;

            param.OracleType = oracleType;
            command.Parameters.Add(param);
        }

        ///// <summary>
        ///// 依次为Command对象的每个Parameter赋值
        ///// </summary>
        ///// <remarks>如果为为REF CURSOR类型，则使用DBNull做临时赋值</remarks>
        ///// <param name="command">Command实例</param>
        ///// <param name="values">待赋值的数组</param>
        //protected override void AssignParameterValues(DbCommand command, params DbParameter[] values)
        //{
        //    if ((values == null) || (values.Length == 0)) return;
        //    int valueShiftIndex = 0;
        //    for (int i = 0; i < command.Parameters.Count; i++)
        //    {
        //        OracleParameter parameter = ((OracleCommand)command).Parameters[i];
        //        if (IsOracleReturnParameter(parameter))
        //            SetParameterValue(command, parameter.ParameterName, Convert.DBNull);
        //        else
        //            SetParameterValue(command, parameter.ParameterName, values[valueShiftIndex++]);
        //    }
        //}

        /// <summary>
        /// 判断某个Parameter是否为REF CURSOR
        /// </summary>
        /// <param name="parameter">Oracle参数对象</param>
        /// <returns></returns>
        protected virtual bool IsOracleReturnParameter(OracleParameter parameter)
        {
            return (((parameter.Direction == ParameterDirection.Output) && (parameter.OracleType == OracleType.Cursor)) ||
                (parameter.Direction == ParameterDirection.ReturnValue));
        }

        /// <summary>
        /// 判断Command对象所需的参数数量是否与待赋值的数组成员数量匹配
        /// </summary>
        /// <remarks>
        /// 对于Oracle存储过程不计入REF Cursor的数量
        /// </remarks>
        /// <param name="command">Command对象</param>
        /// <param name="values">待赋值的数组</param>
        /// <returns>是否匹配</returns>
        protected override bool SameNumberOfParametersAndValues(DbCommand command, object[] values)
        {
            if (command.CommandType != CommandType.StoredProcedure)
                return base.SameNumberOfParametersAndValues(command, values);

            return command.Parameters.Count == (values.Length + GetNonRefCursorParameterCount((OracleCommand)command));
        }

        /// <summary>
        /// 获得OracleCommand对象中所有非REF Cursor类型参数的数量
        /// </summary>
        /// <param name="command">OracleCommand实例</param>
        /// <returns>数量</returns>
        private int GetNonRefCursorParameterCount(OracleCommand command)
        {
            int count = 0;
            if (command == null)
            {
                count = -1;
            }
            else if ((command.Parameters == null) || (command.Parameters.Count == 0))
            {
                count = 0;
            }
            else
            {
                foreach (OracleParameter parameter in command.Parameters)
                {
                    if (IsOracleReturnParameter(parameter))
                        count++;
                }
            }
            return count;
        }
        #endregion

        #region Private Methods
        private void RegisterParameterType(DbCommand command, string parameterName, DbType dbType)
        {
            ParameterTypeRegistry registry = GetParameterTypeRegistry(command.CommandText);
            if (registry == null)
            {
                registry = new ParameterTypeRegistry();
                parameterTypesRegistry.Add(command.CommandText, registry);
            }
            registry.Registry(parameterName, dbType);
        }

        private ParameterTypeRegistry GetParameterTypeRegistry(string commandText)
        {
            ParameterTypeRegistry registry;
            parameterTypesRegistry.TryGetValue(commandText, out registry);
            return registry;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// 判断Oracle存储过程中是否包括Curor参数
        /// </summary>
        /// <remarks>Oracle中的Ref Cursor型参数相当于T-SQL 中的SELECT返回结果</remarks>
        /// <param name="command"></param>
        /// <returns></returns>
        private static bool StoredProcedureNeedsCursorParameter(DbCommand command)
        {
            foreach (OracleParameter parameter in command.Parameters)
                if (parameter.OracleType == OracleType.Cursor)
                    return false;
            return true;
        }

        /// <summary>
        /// 为Oracle存储过程准备Curor参数
        /// </summary>
        /// <remarks>Oracle中的Ref Cursor型参数相当于T-SQL 中的SELECT返回结果</remarks>
        /// <param name="command">命令对象</param>
        private void PrepareRefCursor(DbCommand command)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(command == null, "command");

            if (command.CommandType == CommandType.StoredProcedure)
                if (StoredProcedureNeedsCursorParameter(command))
                    AddParameter(command as OracleCommand, DeluxeWorksOracleCursorNameRoot, OracleType.Cursor, 0,
                        ParameterDirection.Output, true, 0, 0, String.Empty, DataRowVersion.Default, Convert.DBNull);
        }

        /// <summary>
        /// 将通常的GUID类型转变成Byte Array，在Oracle中存/取
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static object ConvertGuidToByteArray(object value)
        {
            return ((value is DBNull) || (value == null)) ? Convert.DBNull : ((Guid)value).ToByteArray();
        }

        //private static object ConvertByteArrayToGuid(object value)
        //{
        //    byte[] buffer = (byte[])value;
        //    if (buffer.Length == 0)
        //        return DBNull.Value;
        //    else
        //        return new Guid(buffer);
        //}

        #endregion

        #region LoadDataSet
        ///// <summary>
        ///// 向DataSet中填充SQL查询返回的结果
        ///// </summary>
        ///// <param name="command">Command对象</param>
        ///// <param name="dataSet">待填充的DataSet</param>
        ///// <param name="tableNames">每个查询结果的DataTable名称</param>
        //public override void LoadDataSet(DbCommand command, DataSet dataSet, params string[] tableNames)
        //{
        //    ExceptionHelper.TrueThrow<NotSupportedException>(command.CommandType == CommandType.Text && tableNames.Length > 1,
        //        Properties.Resource.OracleMultiTablesError);

        //    PrepareRefCursor(command);

        //    base.LoadDataSet(command, dataSet, tableNames);
        //}
        #endregion

        #region ExecuteReader
        /// <summary>
        /// 返回一个DataReader对象
        /// </summary>
        /// <remarks>
        /// <list>
        ///     <item>对于存储过程方式返回DataReader需包括有返回查询结果的情况</item>
        ///     <item>需要外部应用显示关闭Reader</item>
        /// </list>
        /// </remarks>
        /// <param name="command">命令实例</param>
        /// <returns>DataReader对象</returns>
        public override DbDataReader ExecuteReader(DbCommand command)
        {
            PrepareRefCursor(command);
            return (DbDataReader)(new OracleDataReaderWrapper((OracleDataReader)base.ExecuteReader(command)));
        }
        #endregion

        #region Stored Procedure Mechanism
        /// <summary>
        /// 根据Command对象指向存储过程获取其所需的参数组
        /// </summary>
        /// <param name="discoveryCommand">Command对象</param>
        protected override void DeriveParameters(DbCommand discoveryCommand)
        {
            OracleCommandBuilder.DeriveParameters((OracleCommand)discoveryCommand);
        }

        #endregion

        #region Batch Event Mechanism added by wangxiang . May 21, 2008

        /// <summary>
        /// 为DataAdapter更新过程设置事件委托
        /// </summary>
        /// <param name="adapter">Data Adapter</param>
        protected override void SetUpRowUpdatedEvent(DbDataAdapter adapter)
        {
            ((OracleDataAdapter)adapter).RowUpdated +=
                new OracleRowUpdatedEventHandler(OnOracleRowUpdated);
        }

        /// <summary>
        /// 对记录更新过程的响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnOracleRowUpdated(object sender, OracleRowUpdatedEventArgs args)
        {
            if (args.RecordsAffected == 0)
            {
                if (args.Errors != null)
                {
                    args.Row.RowError = Resource.ExceptionMessageUpdateDataSetRowFailure;
                    args.Status = UpdateStatus.SkipCurrentRow;
                }
            }
        }

        #endregion
    }
}
