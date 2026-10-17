using System;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// 包含异常的错误定义
    /// </summary>
    public interface IExceptionalError : IError
    {
        /// <summary>
        /// 异常
        /// </summary>
        Exception Exception { get; }

    }
}