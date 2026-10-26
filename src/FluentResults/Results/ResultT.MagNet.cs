using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public partial class Result<TValue>
    {
        /// <summary>
        ///     Implicit conversion of a string to a failed <see cref="Result{TValue}"/>.
        ///     The string is used as the error message and the value stays default.
        /// </summary>
        public static implicit operator Result<TValue>(string errorMessage)
            => errorMessage is null
                ? Result.Fail<TValue>(Result.DefaultErrorMessage)
                : Result.Fail<TValue>(errorMessage);

        /// <summary>
        ///     Implicit conversion of an exception to a failed <see cref="Result{TValue}"/>.
        ///     The exception is preserved in an <see cref="ExceptionalError"/> and the value stays default.
        /// </summary>
        public static implicit operator Result<TValue>(Exception exception)
            => exception is null
                ? Result.Fail<TValue>(Result.DefaultErrorMessage)
                : Result.Fail<TValue>(new ExceptionalError(exception));

        /// <summary>
        ///     Implicit conversion of a <see cref="Result{TValue}"/> to its value.
        ///     Uses <see cref="ValueOrDefault"/>, so a failed result yields the default value.
        /// </summary>
        public static implicit operator TValue(Result<TValue> result)
            => result is null ? default : result.ValueOrDefault;

        /// <summary>
        ///     Implicit conversion of a <see cref="Result{TValue}"/> to the exception it represents.
        ///     A successful result yields <see cref="Result.DefaultError"/>.
        /// </summary>
        public static implicit operator Exception(Result<TValue> result)
            => ResultHelper.ToException(result);

        /// <summary>
        ///     Implicit conversion of a <see cref="Result{TValue}"/> to a <see cref="Result"/> without a value.
        ///     The reasons are preserved; the value is dropped.
        /// </summary>
        public static implicit operator Result(Result<TValue> result)
            => result == null ? null : result.ToResult();

        /// <summary>
        ///     Merges this result with the given results into a result containing the merged value list.
        /// </summary>
        /// <param name="results">The results to merge with this result</param>
        public Result<IEnumerable<TValue>> Merge(params Result<TValue>[] results)
            => Result.Merge(new[] { this }.Concat(results).ToArray());
    }
}
