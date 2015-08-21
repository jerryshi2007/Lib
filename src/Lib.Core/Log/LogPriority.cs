using System;

namespace Lib.Log
{
    /// <summary>
    /// 日志优先级
    /// </summary>
    /// <remarks>
    /// 共分五级优先级
    /// </remarks>
    public enum LogPriority
    {
        /// <summary>
        /// 最低
        /// </summary>
        Lowest,

        /// <summary>
        /// 低
        /// </summary>
        BelowNormal,

        /// <summary>
        /// 普通
        /// </summary>
        Normal,

        /// <summary>
        /// 高
        /// </summary>
        AboveNormal,

        /// <summary>
        /// 最高
        /// </summary>
        Highest
    }
}
