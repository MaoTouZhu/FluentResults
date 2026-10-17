using System;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// 用于创建和处理结果元素的设置
    /// </summary>
    public class ResultSettings
    {
        /// <summary>
        /// 设置 ResultLogger
        /// </summary>
        public IResultLogger Logger { get; set; }

        /// <summary>
        /// 用于创建 IError 对象的工厂。在 Try 方法中创建错误的所有场景中使用
        /// </summary>
        public Func<Exception, IError> DefaultTryCatchHandler { get; set; }

        /// <summary>
        /// 用于创建 ISuccess 对象的工厂。在 FluentResults 中创建成功的所有场景中使用。 
        /// </summary>
        public Func<string, ISuccess> SuccessFactory { get; set; }

        /// <summary>
        /// 用于创建 IError 对象的工厂。在 FluentResults 中创建错误的所有场景中使用。 
        /// </summary>
        public Func<string, IError> ErrorFactory { get; set; }

        /// <summary>
        /// 用于创建 IExceptionalError 对象的工厂。在 FluentResults 中创建异常错误的所有场景中使用。 
        /// </summary>
        public Func<string, Exception, IExceptionalError> ExceptionalErrorFactory { get; set; }
    }
}