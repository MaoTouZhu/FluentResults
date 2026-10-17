using System;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    ///     MagNet 风格的工厂辅助方法。
    /// </summary>
    public static class MagNetSugarExtensions
    {
        /// <summary>
        ///     根据错误消息创建一个失败的 <see cref="Result"/>。
        /// </summary>
        public static Result ToError(this string errorMessage)
            => errorMessage is null ? Result.Fail(Result.DefaultErrorMessage) : Result.Fail(errorMessage);

        /// <summary>
        ///     根据错误消息创建一个失败的 <see cref="Result{TValue}"/>。
        /// </summary>
        public static Result<TValue> ToError<TValue>(this string errorMessage)
            => errorMessage is null
                ? Result.Fail<TValue>(Result.DefaultErrorMessage)
                : Result.Fail<TValue>(errorMessage);

        /// <summary>
        ///     根据异常创建一个失败的 <see cref="Result"/>。
        ///     该异常保存在 <see cref="ExceptionalError"/> 中。
        /// </summary>
        public static Result ToError(this Exception exception)
            => exception is null
                ? Result.Fail(Result.DefaultErrorMessage)
                : Result.Fail(new ExceptionalError(exception));

        /// <summary>
        ///     根据异常创建一个失败的 <see cref="Result{TValue}"/>。
        ///     该异常保存在 <see cref="ExceptionalError"/> 中。
        /// </summary>
        public static Result<TValue> ToError<TValue>(this Exception exception)
            => exception is null
                ? Result.Fail<TValue>(Result.DefaultErrorMessage)
                : Result.Fail<TValue>(new ExceptionalError(exception));
    }
}
