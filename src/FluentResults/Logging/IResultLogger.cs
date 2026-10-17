// ReSharper disable once CheckNamespace

using Microsoft.Extensions.Logging;

namespace FluentResults
{
    /// <summary>
    /// 日志接口。如果希望对结果进行自定义日志记录，请实现此接口
    /// </summary>
    public interface IResultLogger
    {
        /// <summary>
        /// 记录结果信息
        /// </summary>
        /// <param name="context">附加的日志上下文</param>
        /// <param name="content">要记录的内容</param>
        /// <param name="result">要记录的结果</param>
        /// <param name="logLevel">日志级别，参见 <see cref="Microsoft.Extensions.Logging.LogLevel"/></param>
        void Log(string context, string content, ResultBase result, LogLevel logLevel);

        /// <summary>
        /// 记录结果信息
        /// </summary>
        /// <typeparam name="TContext">附加的日志上下文</typeparam>
        /// <param name="content">要记录的内容</param>
        /// <param name="result">要记录的结果</param>
        /// <param name="logLevel">日志级别，参见 <see cref="Microsoft.Extensions.Logging.LogLevel"/></param>
        void Log<TContext>(string content, ResultBase result, LogLevel logLevel);
    }
}