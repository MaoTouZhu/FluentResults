using System;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public partial class Result<TValue>
    {
        /// <summary>
        ///     从字符串到失败的 <see cref="Result{TValue}"/> 的隐式转换。
        ///     该字符串用作错误消息，值保持为默认值。
        /// </summary>
        public static implicit operator Result<TValue>(string errorMessage)
            => errorMessage is null
                ? Result.Fail<TValue>(Result.DefaultErrorMessage)
                : Result.Fail<TValue>(errorMessage);

        /// <summary>
        ///     从异常到失败的 <see cref="Result{TValue}"/> 的隐式转换。
        ///     该异常保存在 <see cref="ExceptionalError"/> 中，值保持为默认值。
        /// </summary>
        public static implicit operator Result<TValue>(Exception exception)
            => exception is null
                ? Result.Fail<TValue>(Result.DefaultErrorMessage)
                : Result.Fail<TValue>(new ExceptionalError(exception));

        /// <summary>
        ///     从 <see cref="Result{TValue}"/> 到其值的隐式转换。
        ///     使用 <see cref="ValueOrDefault"/>，因此失败的结果会返回默认值。
        /// </summary>
        public static implicit operator TValue(Result<TValue> result)
            => result is null ? default : result.ValueOrDefault;

        /// <summary>
        ///     从 <see cref="Result{TValue}"/> 到其成功状态的隐式转换。
        /// </summary>
        public static implicit operator bool(Result<TValue> result)
            => result != null && result.IsSuccess;

        /// <summary>
        ///     从 <see cref="Result{TValue}"/> 到其表示的异常的隐式转换。
        ///     成功的结果会返回 <see cref="Result.DefaultError"/>。
        /// </summary>
        public static implicit operator Exception(Result<TValue> result)
            => ResultHelper.ToException(result);

        /// <summary>
        ///     从 <see cref="Result{TValue}"/> 到不带值的 <see cref="Result"/> 的隐式转换。
        ///     原因保持不变；值被丢弃。
        /// </summary>
        public static implicit operator Result(Result<TValue> result)
            => result == null ? null : result.ToResult();
    }
}
