using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// 原因的定义。这也是 ISuccess 和 IError 的基础
    /// </summary>
    public interface IReason
    {
        /// <summary>
        /// 原因的消息
        /// </summary>
        string Message { get; }

        /// <summary>
        /// 添加到原因的任何元数据
        /// </summary>
        Dictionary<string, object> Metadata { get; }
    }
}