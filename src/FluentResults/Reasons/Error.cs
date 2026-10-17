using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// Error 类的对象会导致失败的结果
    /// </summary>
    public class Error : IError
    {
        /// <summary>
        /// 错误消息
        /// </summary>
        public string Message { get; protected set; }

        /// <summary>
        /// 错误的元数据
        /// </summary>
        public Dictionary<string, object> Metadata { get; }

        /// <summary>
        /// 获取错误的原因
        /// </summary>
        public List<IError> Reasons { get; }

        /// <summary>
        /// 创建 <see cref="Error"/> 的新实例
        /// </summary>
        protected Error()
        {
            Metadata = new Dictionary<string, object>();
            Reasons = new List<IError>();
        }

        /// <summary>
        /// 创建 <see cref="Error"/> 的新实例
        /// </summary>
        /// <param name="message">错误的描述</param>
        public Error(string message)
            : this()
        {
            Message = message;
        }

        /// <summary>
        /// 创建 <see cref="Error"/> 的新实例
        /// </summary>
        /// <param name="message">错误的描述</param>
        /// <param name="causedBy"><see cref="Error"/> 的根因</param>
        public Error(string message, IError causedBy)
            : this(message)
        {
            if (causedBy == null)
                throw new ArgumentNullException(nameof(causedBy));

            Reasons.Add(causedBy);
        }

        /// <summary>
        /// 设置错误的根因
        /// </summary>
        public Error CausedBy(IError error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            Reasons.Add(error);
            return this;
        }

        /// <summary>
        /// 设置错误的根因
        /// </summary>
        public Error CausedBy(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            Reasons.Add(Result.Settings.ExceptionalErrorFactory(null, exception));
            return this;
        }

        /// <summary>
        /// 设置错误的根因
        /// </summary>
        public Error CausedBy(string message, Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            Reasons.Add(Result.Settings.ExceptionalErrorFactory(message, exception));
            return this;
        }

        /// <summary>
        /// 设置错误的根因
        /// </summary>
        public Error CausedBy(string message)
        {
            Reasons.Add(Result.Settings.ErrorFactory(message));
            return this;
        }

        /// <summary>
        /// 设置错误的根因
        /// </summary>
        public Error CausedBy(IEnumerable<IError> errors)
        {
            if (errors == null)
                throw new ArgumentNullException(nameof(errors));

            Reasons.AddRange(errors);
            return this;
        }

        /// <summary>
        /// 设置错误的根因
        /// </summary>
        public Error CausedBy(IEnumerable<string> errors)
        {
            if (errors == null)
                throw new ArgumentNullException(nameof(errors));

            Reasons.AddRange(errors.Select(errorMessage => Result.Settings.ErrorFactory(errorMessage)));
            return this;
        }

        /// <summary>
        /// 设置元数据
        /// </summary>
        public Error WithMetadata(string metadataName, object metadataValue)
        {
            Metadata.Add(metadataName, metadataValue);
            return this;
        }

        /// <summary>
        /// 设置元数据
        /// </summary>
        public Error WithMetadata(Dictionary<string, object> metadata)
        {
            foreach (var metadataItem in metadata)
            {
                Metadata.Add(metadataItem.Key, metadataItem.Value);
            }

            return this;
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
                .Build();
        }
    }

    internal class ReasonFormat
    {
        public static string ErrorReasonsToString(IReadOnlyCollection<IError> errorReasons)
        {
            return string.Join("; ", errorReasons);
        }

        public static string ReasonsToString(IReadOnlyCollection<IReason> errorReasons)
        {
            return string.Join("; ", errorReasons);
        }
    }
}
