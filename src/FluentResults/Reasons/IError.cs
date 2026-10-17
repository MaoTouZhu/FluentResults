using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// 错误的定义
    /// </summary>
    public interface IError : IReason
    {
        /// <summary>
        /// 错误的原因
        /// </summary>
        List<IError> Reasons { get; }
    }
}