using System;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public partial class Result
    {
        /// <summary>
        ///     Default error message used when a failed result has no message.
        /// </summary>
        public const string DefaultErrorMessage = "Undefined error";

        private static readonly Exception DefaultErrorInstance = new Exception(DefaultErrorMessage);

        /// <summary>
        ///     Default exception thrown by <see cref="ResultBase"/>.<c>Throw()</c> for a successful result.
        /// </summary>
        public static Exception DefaultError => DefaultErrorInstance;

        /// <summary>
        ///     Implicit conversion of a string to a failed <see cref="Result"/>.
        ///     The string is used as the error message.
        /// </summary>
        public static implicit operator Result(string errorMessage)
            => errorMessage is null ? Fail(DefaultErrorMessage) : Fail(errorMessage);

        /// <summary>
        ///     Implicit conversion of an exception to a failed <see cref="Result"/>.
        ///     The exception is preserved in an <see cref="ExceptionalError"/>.
        /// </summary>
        public static implicit operator Result(Exception exception)
            => exception is null ? Fail(DefaultErrorMessage) : Fail(new ExceptionalError(exception));

        /// <summary>
        ///     Implicit conversion of a boolean to a <see cref="Result"/>.
        ///     True creates a success; false creates a failure with <see cref="DefaultErrorMessage"/>.
        /// </summary>
        public static implicit operator Result(bool isSuccess)
            => isSuccess ? Ok() : Fail(DefaultErrorMessage);

        /// <summary>
        ///     Implicit conversion of a <see cref="Result"/> to the exception it represents.
        ///     A successful result yields <see cref="DefaultError"/>.
        /// </summary>
        public static implicit operator Exception(Result result)
            => ResultHelper.ToException(result);

        /// <summary>
        ///     Merges this result with the given results into one result.
        /// </summary>
        /// <param name="results">The results to merge with this result</param>
        public Result Merge(params IResultBase[] results)
            => Result.Merge(new ResultBase[] { this }.Concat(results.OfType<ResultBase>()).ToArray());
    }
}
