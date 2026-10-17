// ReSharper disable once CheckNamespace

using Microsoft.Extensions.Logging;

namespace FluentResults
{
    /// <summary>
    /// <see cref="IResultLogger"/> 的默认实现，不记录任何内容
    /// </summary>
    public class DefaultLogger : IResultLogger
    {
        /// <inheritdoc/>
        public void Log(string context, string content, ResultBase result, LogLevel logLevel)
        {

        }

        /// <inheritdoc/>
        public void Log<TContext>(string content, ResultBase result, LogLevel logLevel)
        {

        }
    }
}