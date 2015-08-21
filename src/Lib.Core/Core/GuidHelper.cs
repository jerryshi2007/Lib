using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lib.Core
{
    public class GuidHelper
    {
        /// <summary>
        /// 32 位：xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        /// </summary>
        /// <returns></returns>
        public static string NewGuid()
        {
            return NewGuid("N");
        }

        /// <summary>
        /// format:
        /// N,32 位：xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
        /// D,由连字符分隔的 32 位数字：xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
        /// B,括在大括号中、由连字符分隔的 32 位数字：{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}
        /// P,括在圆括号中、由连字符分隔的 32 位数字：(xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx)
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string NewGuid(string format)
        {
            return Guid.NewGuid().ToString(format);
        }
    }
}
