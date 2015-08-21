using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Lib.Web
{
    public class ResourceUtility
    {
        #region From WebApp

        public static string GetFileString(string filePath)
        {
            Stream stream = GetFileStream(filePath);
            StreamReader sr = new StreamReader(stream);
            try
            {
                return sr.ReadToEnd();
            }
            finally
            {
                sr.Close();
            }
        }

        public static Stream GetFileStream(string filePath)
        {
            Stream stream = new FileStream(filePath, FileMode.Open);
            return stream;
        }

        #endregion

        #region From Assembly

        /// <summary>
        /// 从资源中读取以内嵌文件方式保存的字符串
        /// </summary>
        /// <param name="strPath">文件的路径</param>
        /// <param name="assembly">Assembly实例</param>
        /// <returns>结果字符串</returns>
        public static string GetResourceString(string strPath, Assembly assembly)
        {
            Stream stream = GetResourceStream(strPath, assembly);

            StreamReader sr = new StreamReader(stream);
            try
            {
                return sr.ReadToEnd();
            }
            finally
            {
                sr.Close();
            }
        }

        /// <summary>
        /// 从资源中读取以内嵌文件方式保存的字符串
        /// </summary>
        /// <param name="strPath">文件的路径</param>
        /// <param name="encoding">字符的编码方式</param>
        /// <param name="assembly">Assembly实例</param>
        /// <returns>结果字符串</returns>
        public static string GetResourceString(string strPath, Encoding encoding, Assembly assembly)
        {
            Stream stream = GetResourceStream(strPath, assembly);

            StreamReader sr = new StreamReader(stream, encoding);
            try
            {
                return sr.ReadToEnd();
            }
            finally
            {
                sr.Close();
            }
        }

        /// <summary>
        /// 从Assembly的资源中得到数据流
        /// </summary>
        /// <param name="strPath">资源的路径，例如：Goo.Data.CustomerInfo</param>
        /// <param name="assembly">Assembly实例</param>
        /// <returns>数据流</returns>
        public static Stream GetResourceStream(string strPath, Assembly assembly)
        {
            Stream stream = assembly.GetManifestResourceStream(strPath);

            return stream;
        }

        #endregion
    }
}
