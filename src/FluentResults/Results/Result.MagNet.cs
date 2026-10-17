using System;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public partial class Result
    {
        /// <summary>
        ///     当失败的结果没有消息时使用的默认错误消息。
        /// </summary>
        public const string DefaultErrorMessage = "Undefined error";

        private static readonly Exception DefaultErrorInstance = new Exception(DefaultErrorMessage);

        /// <summary>
        ///     对于成功的结果，由 <see cref="ResultBase"/>.<c>Throw()</c> 抛出的默认异常。
        /// </summary>
        public static Exception DefaultError => DefaultErrorInstance;

        /// <summary>
        ///     从字符串到失败的 <see cref="Result"/> 的隐式转换。
        ///     该字符串用作错误消息。
        /// </summary>
        public static implicit operator Result(string errorMessage)
            => errorMessage is null ? Fail(DefaultErrorMessage) : Fail(errorMessage);

        /// <summary>
        ///     从异常到失败的 <see cref="Result"/> 的隐式转换。
        ///     该异常保存在 <see cref="ExceptionalError"/> 中。
        /// </summary>
        public static implicit operator Result(Exception exception)
            => exception is null ? Fail(DefaultErrorMessage) : Fail(new ExceptionalError(exception));

        /// <summary>
        ///     从布尔值到 <see cref="Result"/> 的隐式转换。
        ///     true 创建成功；false 创建带有 <see cref="DefaultErrorMessage"/> 的失败。
        /// </summary>
        public static implicit operator Result(bool isSuccess)
            => isSuccess ? Ok() : Fail(DefaultErrorMessage);

        /// <summary>
        ///     从 <see cref="Result"/> 到其成功状态的隐式转换。
        /// </summary>
        public static implicit operator bool(Result result)
            => result != null && result.IsSuccess;

        /// <summary>
        ///     从 <see cref="Result"/> 到其表示的异常的隐式转换。
        ///     成功的结果会返回 <see cref="DefaultError"/>。
        /// </summary>
        public static implicit operator Exception(Result result)
            => ResultHelper.ToException(result);
    }
}
