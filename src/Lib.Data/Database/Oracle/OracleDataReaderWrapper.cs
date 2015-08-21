using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Lib.Data
{
    /// <summary>
    /// OracleDataReader的包装类
    /// </summary>
    /// <remarks>主要用于Oracle体系不同属类型与DbType之间转换的</remarks>
    public class OracleDataReaderWrapper : MarshalByRefObject, IDataReader, IEnumerable
    {
        #region Private Fields
        private DbDataReader innerReader;
        #endregion

        #region Operator Overload
        /// <summary>
        /// 隐式的类型转换
        /// </summary>
        /// <param name="oracleDataReaderWrapper"></param>
        /// <returns></returns>
        public static explicit operator DbDataReader(OracleDataReaderWrapper oracleDataReaderWrapper)
        {
            return oracleDataReaderWrapper.InnerReader;
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Oracle数据Reader的转换器
        /// </summary>
        /// <param name="reader"></param>
        public OracleDataReaderWrapper(DbDataReader reader)
        {
            this.innerReader = reader;
        }
        #endregion

        #region Implements IDataReader, IEnumerable
        /// <summary>
        /// 数组索引器 -- 位置
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                return InnerReader[index];
            }
        }

        /// <summary>
        /// 数组索引器 -- name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object this[string name]
        {
            get
            {
                return InnerReader[name];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)InnerReader).GetEnumerator();
        }

        /// <summary>
        /// 实现IDisposable.Dispose
        /// </summary>
        void IDisposable.Dispose()
        {
            InnerReader.Dispose();
        }

        /// <summary>
        /// Reader对象的关闭
        /// </summary>
        public void Close()
        {
            InnerReader.Close();
        }

        /// <summary>
        /// 获取Reader对象的SchemaTable
        /// </summary>
        /// <returns></returns>
        public DataTable GetSchemaTable()
        {
            return InnerReader.GetSchemaTable();
        }

        /// <summary>
        /// 读取下一行结果
        /// </summary>
        /// <returns></returns>
        public bool NextResult()
        {
            return InnerReader.NextResult();
        }

        /// <summary>
        /// 实现IDataReader的Read对象
        /// </summary>
        /// <returns></returns>
        public bool Read()
        {
            return InnerReader.Read();
        }

        /// <summary>
        /// DataReader的Depth
        /// </summary>
        public int Depth
        {
            get
            {
                return InnerReader.Depth;
            }
        }

        /// <summary>
        /// DataReader的IsClosed
        /// </summary>
        public bool IsClosed
        {
            get
            {
                return InnerReader.IsClosed;
            }
        }

        /// <summary>
        /// DataReader的RecordsAffected
        /// </summary>
        public int RecordsAffected
        {
            get
            {
                return InnerReader.RecordsAffected;
            }
        }

        /// <summary>
        /// DataReader的FieldCount
        /// </summary>
        public int FieldCount
        {
            get
            {
                return InnerReader.FieldCount;
            }
        }

        /// <summary>
        /// 获取Char类型的值
        /// </summary>
        /// <param name="index">列序号</param>
        /// <returns>Char类型的值</returns>
        public Char GetChar(int index)
        {
            return InnerReader.GetChar(index);
        }

        /// <summary>
        /// 获取DataReader对象
        /// </summary>
        /// <param name="index">列序号</param>
        /// <returns>DataReader对象</returns>
        public IDataReader GetData(int index)
        {
            return InnerReader.GetData(index);
        }

        /// <summary>
        /// 获得数据类型名称
        /// </summary>
        /// <param name="index">列序号</param>
        /// <returns>数据类型名称</returns>
        public string GetDataTypeName(int index)
        {
            return InnerReader.GetDataTypeName(index);
        }

        /// <summary>
        /// DataReader的GetDateTime
        /// </summary>
        /// <param name="ordinal_">列序号</param>
        /// <returns>DateTime值</returns>
        public DateTime GetDateTime(int ordinal_)
        {
            return InnerReader.GetDateTime(ordinal_);
        }

        /// <summary>
        /// DataReader的GetDecimal
        /// </summary>
        /// <param name="index">列序号</param>
        /// <returns>Decimal数据</returns>
        public decimal GetDecimal(int index)
        {
            return InnerReader.GetDecimal(index);
        }

        /// <summary>
        /// 获取双精度浮点值
        /// </summary>
        /// <param name="index">列序号</param>
        /// <returns>双精度浮点值</returns>
        public double GetDouble(int index)
        {
            return InnerReader.GetDouble(index);
        }

        /// <summary>
        /// 获取指定数据类型
        /// </summary>
        /// <param name="index">列序号</param>
        /// <returns>数据类型</returns>
        public Type GetFieldType(int index)
        {
            return InnerReader.GetFieldType(index);
        }

        /// <summary>
        /// 获取16位整形值
        /// </summary>
        /// <param name="index">列序号</param>
        /// <returns>整形值</returns>
        public short GetInt16(int index)
        {
            return Convert.ToInt16(InnerReader[index], CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 获取32位整形值
        /// </summary>
        /// <param name="index">列序号</param>
        /// <returns>整形值</returns>
        public int GetInt32(int index)
        {
            return InnerReader.GetInt32(index);
        }

        /// <summary>
        /// 获取64位整形值
        /// </summary>
        /// <param name="index">列序号</param>
        /// <returns>整形值</returns>
        public long GetInt64(int index)
        {
            return InnerReader.GetInt64(index);
        }

        /// <summary>
        /// 获取对应列的列名称
        /// </summary>
        /// <param name="index">序列号</param>
        /// <returns>列名称</returns>
        public string GetName(int index)
        {
            return InnerReader.GetName(index);
        }

        /// <summary>
        /// 获取对应列的序列号
        /// </summary>
        /// <param name="index">列名称</param>
        /// <returns>序列号</returns>
        public int GetOrdinal(string index)
        {
            return InnerReader.GetOrdinal(index);
        }

        /// <summary>
        /// 获取指定实例的值
        /// </summary>
        /// <param name="index">列序号</param>
        /// <returns>需要的字符串</returns>
        public string GetString(int index)
        {
            return InnerReader.GetString(index);
        }

        /// <summary>
        /// 获取指定实例的值
        /// </summary>
        /// <param name="index">列序号</param>
        /// <returns>指定实例</returns>
        public object GetValue(int index)
        {
            return InnerReader.GetValue(index);
        }

        /// <summary>
        /// 获取集合中所有属性列
        /// </summary>
        /// <param name="values">对象数组</param>
        /// <returns></returns>
        public int GetValues(object[] values)
        {
            return InnerReader.GetValues(values);
        }

        /// <summary>
        /// 判断是否存在不存在或已经丢失的值
        /// </summary>
        /// <param name="index">列序号</param>
        /// <returns></returns>
        public bool IsDBNull(int index)
        {
            return InnerReader.IsDBNull(index);
        }

        /// <summary>
        /// DataReader的InnerReader
        /// </summary>
        public DbDataReader InnerReader
        {
            get
            {
                return this.innerReader;
            }
        }

        /// <summary>
        /// DataReader的GetChars
        /// </summary>
        /// <param name="index">列序号</param>
        /// <param name="dataIndex">索引</param>
        /// <param name="buffer">目标缓冲区</param>
        /// <param name="bufferIndex">缓冲区索引</param>
        /// <param name="length">截取长度</param>
        /// <returns>字节流</returns>
        public long GetChars(int index, long dataIndex, char[] buffer, int bufferIndex, int length)
        {
            return InnerReader.GetChars(index, dataIndex, buffer, bufferIndex, length);
        }

        /// <devdoc>
        /// 0是false，其他的map为true。
        /// </devdoc>        
        /// <summary>
        /// DataReader的GetBoolean
        /// </summary>
        /// <param name="index">对应的位置</param>
        /// <returns>0是false，其他的map为true。</returns>
        public bool GetBoolean(int index)
        {
            return Convert.ToBoolean(InnerReader[index], CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// DataReader的GetByte
        /// </summary>
        /// <param name="index">对应的位置</param>
        /// <returns>0是false，其他的map为true。</returns>
        public byte GetByte(int index)
        {
            return Convert.ToByte(InnerReader[index], CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// DataReader的GetBytes
        /// </summary>
        /// <param name="ordinal">列序号</param>
        /// <param name="dataIndex">索引</param>
        /// <param name="buffer">数据流</param>
        /// <param name="bufferIndex">缓冲区索引</param>
        /// <param name="length">长度</param>
        /// <returns>字节流</returns>
        public long GetBytes(int ordinal, long dataIndex, byte[] buffer, int bufferIndex, int length)
        {
            return InnerReader.GetBytes(ordinal, dataIndex, buffer, bufferIndex, length);
        }

        /// <devdoc>
        /// 由于Oracle对于Double和Float均返回为Decimal，因此这里仅作近似提取
        /// </devdoc> 
        /// <summary>
        /// 由于Oracle对于Double和Float均返回为Decimal，因此这里仅作近似提取
        /// </summary>
        /// <param name="index">对应的位置</param>
        /// <returns>dataReader中所在位置数据</returns>
        public float GetFloat(int index)
        {
            return InnerReader.GetFloat(index);
        }

        /// <devdoc>
        /// Oracle中如果需要使用Guid类型，那么需要定义为Raw(16)
        /// </devdoc>  
        /// <summary>
        ///  Oracle中如果需要使用Guid类型，那么需要定义为Raw(16)
        /// </summary>
        /// <param name="index">对应的位置</param>
        /// <returns>Guid对象</returns>
        public Guid GetGuid(int index)
        {
            byte[] guidBuffer = (byte[])InnerReader[index];
            return new Guid(guidBuffer);
        }
        #endregion
    }
}
