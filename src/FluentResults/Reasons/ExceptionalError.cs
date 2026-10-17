using System;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// 额外存储异常的错误类
    /// </summary>
    public class ExceptionalError : Error, IExceptionalError
    {
        /// <summary>
        /// 错误的异常
        /// </summary>
        public Exception Exception { get; }
        

        /// <summary>
        /// 使用异常初始化新实例
        /// </summary>
        /// <param name="exception">异常</param>
        public ExceptionalError(Exception exception)
            : this(exception.Message, exception)
        { }

        /// <summary>
        /// 使用自定义消息和异常初始化新实例
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="exception">异常</param>
        public ExceptionalError(string message, Exception exception)
            : base(message)
        {
            Exception = exception;
        }

        /// <summary>
        /// 重写的 ToString
        /// </summary>
        public override string ToString()
        {
            return new ReasonStringBuilder()
                .WithReasonType(GetType())
                .WithInfo(nameof(Message), Message)
                .WithInfo(nameof(Metadata), string.Join("; ", Metadata))
                .WithInfo(nameof(Reasons), ReasonFormat.ErrorReasonsToString(Reasons))
                .WithInfo(nameof(Exception), Exception.ToString())
                .Build();
        }
    }
}