using System;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    ///     MagNet-style factory helpers.
    /// </summary>
    public static class MagNetSugarExtensions
    {
        /// <summary>
        ///     Creates a failed <see cref="Result"/> from an error message.
        /// </summary>
        public static Result ToError(this string errorMessage)
            => errorMessage is null ? Result.Fail(Result.DefaultErrorMessage) : Result.Fail(errorMessage);

        /// <summary>
        ///     Creates a failed <see cref="Result{TValue}"/> from an error message.
        /// </summary>
        public static Result<TValue> ToError<TValue>(this string errorMessage)
            => errorMessage is null
                ? Result.Fail<TValue>(Result.DefaultErrorMessage)
                : Result.Fail<TValue>(errorMessage);

        /// <summary>
        ///     Creates a failed <see cref="Result"/> from an exception.
        ///     The exception is preserved in an <see cref="ExceptionalError"/>.
        /// </summary>
        public static Result ToError(this Exception exception)
            => exception is null
                ? Result.Fail(Result.DefaultErrorMessage)
                : Result.Fail(new ExceptionalError(exception));

        /// <summary>
        ///     Creates a failed <see cref="Result{TValue}"/> from an exception.
        ///     The exception is preserved in an <see cref="ExceptionalError"/>.
        /// </summary>
        public static Result<TValue> ToError<TValue>(this Exception exception)
            => exception is null
                ? Result.Fail<TValue>(Result.DefaultErrorMessage)
                : Result.Fail<TValue>(new ExceptionalError(exception));
    }
}
