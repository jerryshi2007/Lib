using Lib.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Lib.Data
{
    public class DataMapping
    {

        #region Object To SQL

        public static string GetTableName<T>()
        {
            TableMappingInfo mappingInfo = TableMappingInfo.GetMappingInfo(typeof(T));
            return mappingInfo.TableName;
        }

        public static string GetSelectText<T>()
        {
            TableMappingInfo mappingInfo = TableMappingInfo.GetMappingInfo(typeof(T));

            return string.Format("SELECT {0} FROM {1}", mappingInfo.FieldInfoCollection.ToFieldString(), mappingInfo.TableName);
        }


        public static string GetInsertText<T>(T obj)
        {
            TableMappingInfo mappingInfo = TableMappingInfo.GetMappingInfo(obj.GetType());

            InsertSqlClauseBuilder builder = new InsertSqlClauseBuilder();
            InsertSqlClauseBuilder<T>(mappingInfo, builder, obj);


            return string.Format("INSERT INTO {0} {1} ", mappingInfo.TableName, builder.ToSqlString(TSqlBuilder.Instance));
        }

        private static void InsertSqlClauseBuilder<T>(TableMappingInfo mappingInfo, SqlClauseBuilderIUW builder, T obj)
        {
            foreach (FieldMappingInfo item in mappingInfo.FieldInfoCollection)
            {
                if (item.IsIdentity == false)
                {
                    object data = GetValueFromObject(item, obj);

                    if (data == null || data == DBNull.Value || (data != null && data.Equals(TypeCreator.GetTypeDefaultValue(data.GetType()))))
                    {
                        if (!string.IsNullOrEmpty(item.DefaultValue))
                        {
                            builder.AppendItem(item.FieldName, item.DefaultValue, SqlClauseBuilderBase.EqualTo, true);
                        }
                    }
                    else
                        builder.AppendItem(item.FieldName, data);
                }
            }
        }

        public static string GetUpdateText<T>(T obj)
        {
            return GetUpdateText(obj, null);
        }

        public static string GetUpdateText<T>(T obj, Dictionary<string,string> ignoreProperty)
        {
            TableMappingInfo mappingInfo = TableMappingInfo.GetMappingInfo(obj.GetType());
            UpdateSqlClauseBuilder builder = new UpdateSqlClauseBuilder();
            WhereSqlClauseBuilder whereBuilder = new WhereSqlClauseBuilder();
            UpdateSqlClauseBuilder<T>(mappingInfo, builder, whereBuilder, obj, ignoreProperty);

            return string.Format("UPDATE {0} SET {1} WHERE {2} ", mappingInfo.TableName, builder.ToSqlString(TSqlBuilder.Instance), whereBuilder.ToSqlString(TSqlBuilder.Instance));
        }

        private static void UpdateSqlClauseBuilder<T>(TableMappingInfo mappingInfo, UpdateSqlClauseBuilder builder, WhereSqlClauseBuilder whereBuilder, T obj, Dictionary<string, string> ignoreProperty)
        {
            foreach (FieldMappingInfo item in mappingInfo.FieldInfoCollection)
            {
                if (ignoreProperty != null)
                {
                    if (ignoreProperty.ContainsKey(item.PropertyName) || ignoreProperty.ContainsValue(item.FieldName))
                        continue;
                }

                if (!item.IsIdentity && !item.IsPrimary)
                {
                    object data = GetValueFromObject(item, obj);

                    //Update时，不应该再获取默认值
                    if(data != null && data != DBNull.Value)
                    {
                        //如果是DateTime类型，默认值为 0001/1/1 00:00:00 ,该情况应该排除
                        if (!(data.GetType().Equals(typeof(DateTime)) && data.Equals(TypeCreator.GetTypeDefaultValue(data.GetType()))))
                        {
                            builder.AppendItem(item.FieldName, data);
                        }
                    }
                }

                if (item.IsPrimary)
                {
                    object data = GetValueFromObject(item, obj);
                    ExceptionHelper.TrueThrow((data == null || data == DBNull.Value), "默认依据主键进行更新或删除，主键值为 NULL");

                    whereBuilder.AppendItem(item.FieldName, data);
                }
            }
        }

        public static string GetDeleteText<T>(T obj)
        {
            TableMappingInfo mappingInfo = TableMappingInfo.GetMappingInfo(obj.GetType());
            WhereSqlClauseBuilder whereBuilder = new WhereSqlClauseBuilder();
            WhereSqlClauseBuilder<T>(mappingInfo, whereBuilder, obj);

            return string.Format("DELETE FROM {0}  WHERE {1}", mappingInfo.TableName, whereBuilder.ToSqlString(TSqlBuilder.Instance));
        }

        private static void WhereSqlClauseBuilder<T>(TableMappingInfo mappingInfo, WhereSqlClauseBuilder whereBuilder, T obj)
        {
            foreach (FieldMappingInfo item in mappingInfo.FieldInfoCollection)
            {
                if (item.IsPrimary)
                {
                    object data = GetValueFromObject(item, obj);

                    ExceptionHelper.TrueThrow((data == null || data == DBNull.Value), "默认依据主键进行更新或删除，主键值为 NULL");

                    whereBuilder.AppendItem(item.FieldName, data);
                }
            }
        }

        #endregion

        #region Data To Object

        public static void DataRowToObject<T>(DataRow row, T obj)
        {
            ExceptionHelper.FalseThrow<ArgumentNullException>(row != null, "row");
            ExceptionHelper.FalseThrow<ArgumentNullException>(obj != null, "obj");
            ExceptionHelper.FalseThrow<ArgumentNullException>(row.Table != null, "row.Table");

            TableMappingInfo mappingInfo = GetMappingInfo(obj.GetType());

            foreach (DataColumn column in row.Table.Columns)
            {
                if (mappingInfo.FieldInfoCollection.Contains(column.ColumnName))
                {
                    FieldMappingInfo item = mappingInfo.FieldInfoCollection[column.ColumnName];

                    System.Type realType = GetRealType(item.MemberInfo);

                    object data = row[column];
                    if (Convertible(realType, data))
                        SetValueToObject(item, obj, data);
                }
            }
        }

        public static void DataReaderToObject<T>(IDataReader reader, T obj)
        {
            ExceptionHelper.FalseThrow<ArgumentNullException>(reader != null, "row");
            ExceptionHelper.FalseThrow<ArgumentNullException>(obj != null, "obj");

            TableMappingInfo mappingInfo = GetMappingInfo(obj.GetType());
            DataTable table = reader.GetSchemaTable();

            foreach (DataRow row in table.Rows)
            {
                string columnName = row["ColumnName"].ToString();
                if (mappingInfo.FieldInfoCollection.Contains(columnName))
                {
                    FieldMappingInfo item = mappingInfo.FieldInfoCollection[columnName];
                    object data = reader[columnName];

                    System.Type realType = GetRealType(item.MemberInfo);

                    if (Convertible(realType, data))
                        SetValueToObject(item, obj, data);
                }
            }
        }

        #endregion

        #region Protected

        protected static TableMappingInfo GetMappingInfo(System.Type type)
        {
            TableMappingInfo mappingInfo = TableMappingInfo.GetMappingInfo(type);
            if (mappingInfo == null)
                ExceptionHelper.TrueThrow<ArgumentNullException>(mappingInfo == null, "mappingInfo");

            return mappingInfo;
        }

        protected static System.Type GetRealType(MemberInfo mi)
        {
            System.Type type = null;

            switch (mi.MemberType)
            {
                case MemberTypes.Property:
                    type = ((PropertyInfo)mi).PropertyType;
                    break;
                case MemberTypes.Field:
                    type = ((FieldInfo)mi).FieldType;
                    break;
                default:

                    break;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition().FullName == "System.Nullable`1")
                type = type.GetGenericArguments()[0];

            return type;
        }

        protected static bool Convertible(System.Type targetType, object data)
        {
            bool result = true;

            if (data == null && targetType.IsValueType)
                result = false;
            else
            {
                if (data == DBNull.Value)
                {
                    if (targetType != typeof(DBNull) && targetType != typeof(string))
                        result = false;
                }
            }

            return result;
        }

        protected static object GetValueFromObject(FieldMappingInfo item, object obj)
        {
            try
            {
                object data = null;

                switch (item.MemberInfo.MemberType)
                {
                    case MemberTypes.Property:
                        PropertyInfo pi = (PropertyInfo)item.MemberInfo;
                        if (pi.CanRead)
                            data = pi.GetValue(obj, null);
                        break;
                    case MemberTypes.Field:
                        FieldInfo fi = (FieldInfo)item.MemberInfo;
                        data = fi.GetValue(obj);
                        break;
                    default:
                        ThrowInvalidMemberInfoTypeException(item.MemberInfo);
                        break;
                }

                return data;
            }
            catch (System.Exception ex)
            {
                System.Exception realEx = ExceptionHelper.GetRealException(ex);

                throw new ApplicationException(string.Format("读取属性{0}值的时候出错，{1}", item.MemberInfo.Name, realEx.Message));
            }
        }

        protected static void SetValueToObject(FieldMappingInfo item, object obj, object data)
        {
            if (data == DBNull.Value)
                data = null;

            data = DecorateDate(data);           

            switch (item.MemberInfo.MemberType)
            {
                case MemberTypes.Property:
                    PropertyInfo pi = (PropertyInfo)item.MemberInfo;
                    if (pi.CanWrite)
                        pi.SetValue(obj, data, null);
                    break;
                case MemberTypes.Field:
                    FieldInfo fi = (FieldInfo)item.MemberInfo;
                    fi.SetValue(obj, data);
                    break;
                default:
                    ThrowInvalidMemberInfoTypeException(item.MemberInfo);
                    break;
            }
        }

        protected static object DecorateDate(object data)
        {
            object result = data;

            if (data is DateTime)
            {
                DateTime dt = (DateTime)data;

                if (dt.Kind == DateTimeKind.Unspecified)
                    result = DateTime.SpecifyKind(dt, DateTimeKind.Local);
            }

            return result;
        }

        #endregion

        #region Private

        private static void ThrowInvalidMemberInfoTypeException(MemberInfo mi)
        {
            throw new InvalidOperationException(string.Format("无效的成员信息：Name:{0}, MemberType:{1}",
                mi.Name,
                mi.MemberType));
        }


        #endregion

    }
}
