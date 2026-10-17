using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    ///     Result 的实现
    /// </summary>
    public partial class Result : ResultBase<Result>
    {
        /// <summary>
        ///     默认构造函数
        /// </summary>
        public Result()
        {
        }

        /// <summary>
        ///     通过 errorMapper 映射结果中的所有错误
        /// </summary>
        /// <param name="errorMapper"></param>
        /// <returns></returns>
        public Result MapErrors(Func<IError, IError> errorMapper)
        {
            if (IsSuccess)
                return this;

            return new Result()
                .WithErrors(Errors.Select(errorMapper))
                .WithSuccesses(Successes);
        }

        /// <summary>
        ///     通过 successMapper 映射结果中的所有成功
        /// </summary>
        /// <param name="successMapper"></param>
        /// <returns></returns>
        public Result MapSuccesses(Func<ISuccess, ISuccess> successMapper)
        {
            return new Result()
                .WithErrors(Errors)
                .WithSuccesses(Successes.Select(successMapper));
        }

        /// <summary>
        ///     将不含值的结果转换为包含值的结果
        /// </summary>
        /// <typeparam name="TNewValue">值的类型</typeparam>
        /// <param name="newValue">要添加到新结果中的值</param>
        public Result<TNewValue> ToResult<TNewValue>(TNewValue newValue = default)
        {
            return new Result<TNewValue>()
                .WithValue(IsFailed ? default : newValue)
                .WithReasons(Reasons);
        }

        /// <summary>
        ///     将结果转换为带值的结果，该转换可能失败。
        /// </summary>
        /// <example>
        ///     <code>
        ///  var bakeryDtoResult = result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="bind">可能失败的转换。</param>
        public Result<TNewValue> Bind<TNewValue>(Func<Result<TNewValue>> bind)
        {
            var result = new Result<TNewValue>();
            result.WithReasons(Reasons);

            if (IsSuccess)
            {
                var converted = bind();
                result.WithValue(converted.ValueOrDefault);
                result.WithReasons(converted.Reasons);
            }

            return result;
        }

        /// <summary>
        ///     将结果异步转换为带值的结果，该转换可能失败。
        /// </summary>
        /// <example>
        ///     <code>
        ///  var bakeryDtoResult = result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="bind">可能失败的转换。</param>
        public async Task<Result<TNewValue>> Bind<TNewValue>(Func<Task<Result<TNewValue>>> bind)
        {
            var result = new Result<TNewValue>();
            result.WithReasons(Reasons);

            if (IsSuccess)
            {
                var converted = await bind();
                result.WithValue(converted.ValueOrDefault);
                result.WithReasons(converted.Reasons);
            }

            return result;
        }

        /// <summary>
        ///     将结果异步转换为带值的结果，该转换可能失败。
        /// </summary>
        /// <example>
        ///     <code>
        ///  var bakeryDtoResult = result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="bind">可能失败的转换。</param>
        public async ValueTask<Result<TNewValue>> Bind<TNewValue>(Func<ValueTask<Result<TNewValue>>> bind)
        {
            var result = new Result<TNewValue>();
            result.WithReasons(Reasons);

            if (IsSuccess)
            {
                var converted = await bind();
                result.WithValue(converted.ValueOrDefault);
                result.WithReasons(converted.Reasons);
            }

            return result;
        }

        /// <summary>
        ///     执行一个返回 <see cref="Result" /> 的操作。
        /// </summary>
        /// <example>
        ///     <code>
        ///  var done = result.Bind(ActionWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="action">可能失败的操作。</param>
        public Result Bind(Func<Result> action)
        {
            var result = new Result();
            result.WithReasons(Reasons);

            if (IsSuccess)
            {
                var converted = action();
                result.WithReasons(converted.Reasons);
            }

            return result;
        }

        /// <summary>
        ///     异步执行一个返回 <see cref="Result" /> 的操作。
        /// </summary>
        /// <example>
        ///     <code>
        ///  var done = result.Bind(ActionWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="action">可能失败的操作。</param>
        public async Task<Result> Bind(Func<Task<Result>> action)
        {
            var result = new Result();
            result.WithReasons(Reasons);

            if (IsSuccess)
            {
                var converted = await action();
                result.WithReasons(converted.Reasons);
            }

            return result;
        }

        /// <summary>
        ///     异步执行一个返回 <see cref="Result" /> 的操作。
        /// </summary>
        /// <example>
        ///     <code>
        ///  var done = result.Bind(ActionWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="action">可能失败的操作。</param>
        public async ValueTask<Result> Bind(Func<ValueTask<Result>> action)
        {
            var result = new Result();
            result.WithReasons(Reasons);

            if (IsSuccess)
            {
                var converted = await action();
                result.WithReasons(converted.Reasons);
            }

            return result;
        }

        /// <summary>
        ///     解构 Result
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="isFailed"></param>
        public void Deconstruct(out bool isSuccess, out bool isFailed)
        {
            isSuccess = IsSuccess;
            isFailed = IsFailed;
        }

        /// <summary>
        ///     解构 Result
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="isFailed"></param>
        /// <param name="errors"></param>
        public void Deconstruct(out bool isSuccess, out bool isFailed, out IReadOnlyList<IError> errors)
        {
            isSuccess = IsSuccess;
            isFailed = IsFailed;
            errors = IsFailed ? Errors : default;
        }

        /// <summary>
        ///     从 <see cref="Error" /> 到 <see cref="Result" /> 的隐式转换
        /// </summary>
        /// <param name="error">错误</param>
        public static implicit operator Result(Error error)
        {
            return Fail(error);
        }

        /// <summary>
        ///     从 <see cref="List{Error}" /> 到 <see cref="Result" /> 的隐式转换
        /// </summary>
        /// <param name="errors">错误</param>
        public static implicit operator Result(List<Error> errors)
        {
            return Fail(errors);
        }
    }

    /// <summary>
    ///     带有类型为 <typeparamref name="TValue" /> 的值的结果的定义
    /// </summary>
    /// <typeparam name="TValue">值的类型</typeparam>
    public interface IResult<out TValue> : IResultBase
    {
        /// <summary>
        ///     获取 Value。如果结果为失败，则会抛出异常，因为失败的结果没有值。相反的情况参见
        ///     属性 ValueOrDefault。
        /// </summary>
        TValue Value { get; }

        /// <summary>
        ///     获取 Value。如果结果为失败，则返回默认值。相反的情况参见属性 Value。
        /// </summary>
        TValue ValueOrDefault { get; }
    }

    /// <summary>
    ///     包含类型为 <typeparamref name="TValue" /> 的值的结果
    /// </summary>
    /// <typeparam name="TValue">值的类型</typeparam>
    public partial class Result<TValue> : ResultBase<Result<TValue>>, IResult<TValue>
    {
        /// <summary>
        ///     默认构造函数
        /// </summary>
        public Result()
        {
        }

        /// <summary>
        ///     <inheritdoc />
        /// </summary>
        public TValue ValueOrDefault { get; private set; }

        /// <summary>
        ///     <inheritdoc />
        /// </summary>
        public TValue Value
        {
            get
            {
                ThrowIfValueUnavailable();

                return ValueOrDefault;
            }
            private set
            {
                ThrowIfValueUnavailable();

                ValueOrDefault = value;
            }
        }

        /// <summary>
        ///     设置值
        /// </summary>
        public Result<TValue> WithValue(TValue value)
        {
            Value = value;
            return this;
        }

        /// <summary>
        ///     通过 errorMapper 映射结果中的所有错误
        /// </summary>
        /// <param name="errorMapper"></param>
        /// <returns></returns>
        public Result<TValue> MapErrors(Func<IError, IError> errorMapper)
        {
            if (IsSuccess)
                return this;

            return new Result<TValue>()
                .WithErrors(Errors.Select(errorMapper))
                .WithSuccesses(Successes);
        }

        /// <summary>
        ///     通过 successMapper 映射结果中的所有成功
        /// </summary>
        /// <param name="successMapper"></param>
        /// <returns></returns>
        public Result<TValue> MapSuccesses(Func<ISuccess, ISuccess> successMapper)
        {
            return new Result<TValue>()
                .WithValue(ValueOrDefault)
                .WithErrors(Errors)
                .WithSuccesses(Successes.Select(successMapper));
        }

        /// <summary>
        ///     将带值的结果转换为不带值的结果
        /// </summary>
        public Result ToResult()
        {
            return new Result()
                .WithReasons(Reasons);
        }

        /// <summary>
        ///     将带值的结果转换为带有另一个值的结果。使用 valueConverter 参数
        ///     指定值的转换逻辑。
        /// </summary>
        public Result<TNewValue> ToResult<TNewValue>(Func<TValue, TNewValue> valueConverter = null)
        {
            return Map(valueConverter);
        }

        /// <summary>
        ///     将带值的结果转换为带有另一个值的结果。使用 valueConverter 参数
        ///     指定值的转换逻辑。
        /// </summary>
        public Result<TNewValue> Map<TNewValue>(Func<TValue, TNewValue> mapLogic)
        {
            if (IsSuccess && mapLogic == null)
                throw new ArgumentException("If result is success then valueConverter should not be null");

            return new Result<TNewValue>()
                .WithValue(IsFailed ? default : mapLogic(Value))
                .WithReasons(Reasons);
        }

        /// <summary>
        ///     将带值的结果转换为带有另一个值的结果，该转换可能失败。
        /// </summary>
        /// <example>
        ///     <code>
        ///  var bakeryDtoResult = result
        ///     .Bind(GetWhichMayFail)
        ///     .Bind(ProcessWhichMayFail)
        ///     .Bind(FormattingWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="bind">可能失败的转换。</param>
        public Result<TNewValue> Bind<TNewValue>(Func<TValue, Result<TNewValue>> bind)
        {
            var result = new Result<TNewValue>();
            result.WithReasons(Reasons);

            if (IsSuccess)
            {
                var converted = bind(Value);
                result.WithValue(converted.ValueOrDefault);
                result.WithReasons(converted.Reasons);
            }

            return result;
        }

        /// <summary>
        ///     将带值的结果异步转换为带有另一个值的结果，该转换可能失败。
        /// </summary>
        /// <example>
        ///     <code>
        ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="bind">可能失败的转换。</param>
        public async Task<Result<TNewValue>> Bind<TNewValue>(Func<TValue, Task<Result<TNewValue>>> bind)
        {
            var result = new Result<TNewValue>();
            result.WithReasons(Reasons);

            if (IsSuccess)
            {
                var converted = await bind(Value);
                result.WithValue(converted.ValueOrDefault);
                result.WithReasons(converted.Reasons);
            }

            return result;
        }

        /// <summary>
        ///     将带值的结果异步转换为带有另一个值的结果，该转换可能失败。
        /// </summary>
        /// <example>
        ///     <code>
        ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="bind">可能失败的转换。</param>
        public async ValueTask<Result<TNewValue>> Bind<TNewValue>(Func<TValue, ValueTask<Result<TNewValue>>> bind)
        {
            var result = new Result<TNewValue>();
            result.WithReasons(Reasons);

            if (IsSuccess)
            {
                var converted = await bind(Value);
                result.WithValue(converted.ValueOrDefault);
                result.WithReasons(converted.Reasons);
            }

            return result;
        }

        /// <summary>
        ///     执行一个返回 <see cref="Result" /> 的操作。
        /// </summary>
        /// <example>
        ///     <code>
        ///  var done = result.Bind(ActionWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="action">可能失败的操作。</param>
        public Result Bind(Func<TValue, Result> action)
        {
            var result = new Result();
            result.WithReasons(Reasons);

            if (IsSuccess)
            {
                var converted = action(Value);
                result.WithReasons(converted.Reasons);
            }

            return result;
        }

        /// <summary>
        ///     异步执行一个返回 <see cref="Result" /> 的操作。
        /// </summary>
        /// <example>
        ///     <code>
        ///  var done = await result.Bind(ActionWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="action">可能失败的操作。</param>
        public async Task<Result> Bind(Func<TValue, Task<Result>> action)
        {
            var result = new Result();
            result.WithReasons(Reasons);

            if (IsSuccess)
            {
                var converted = await action(Value);
                result.WithReasons(converted.Reasons);
            }

            return result;
        }

        /// <summary>
        ///     异步执行一个返回 <see cref="Result" /> 的操作。
        /// </summary>
        /// <example>
        ///     <code>
        ///  var done = await result.Bind(ActionWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="action">可能失败的操作。</param>
        public async ValueTask<Result> Bind(Func<TValue, ValueTask<Result>> action)
        {
            var result = new Result();
            result.WithReasons(Reasons);

            if (IsSuccess)
            {
                var converted = await action(Value);
                result.WithReasons(converted.Reasons);
            }

            return result;
        }

        /// <summary>
        ///     ToString 实现
        /// </summary>
        public override string ToString()
        {
            var baseString = base.ToString();
            var valueString = ValueOrDefault.ToLabelValueStringOrEmpty(nameof(Value));
            return $"{baseString}, {valueString}";
        }

        /// <summary>
        ///     从不带值的 <see cref="Result" /> 到具有默认值的 <see cref="Result{TValue}" />
        ///     的隐式转换
        /// </summary>
        public static implicit operator Result<TValue>(Result result)
        {
            return result.ToResult<TValue>();
        }

        /// <summary>
        ///     从具有值的 <see cref="Result{TValue}" /> 到不带值的 <see cref="Result" /> 的隐式转换
        /// </summary>
        public static implicit operator Result<object>(Result<TValue> result)
        {
            return result.ToResult<object>(value => value);
        }

        /// <summary>
        ///     从值到 <see cref="Result{TValue}" /> 的隐式转换
        /// </summary>
        public static implicit operator Result<TValue>(TValue value)
        {
            if (value is Result<TValue> r)
                return r;

            return Result.Ok(value);
        }

        /// <summary>
        ///     从 <see cref="Error" /> 到 <see cref="Result{TValue}" /> 的隐式转换
        /// </summary>
        public static implicit operator Result<TValue>(Error error)
        {
            return Result.Fail(error);
        }

        /// <summary>
        ///     从 <see cref="Error" /> 列表到 <see cref="Result{TValue}" /> 的隐式转换
        /// </summary>
        public static implicit operator Result<TValue>(List<Error> errors)
        {
            return Result.Fail(errors);
        }

        /// <summary>
        ///     解构 Result
        /// </summary>
        /// <param name="value"></param>
        /// <param name="errors"></param>
        public void Deconstruct(out TValue value, out IReadOnlyList<IError> errors)
        {
            value = IsSuccess ? Value : default;
            errors = IsFailed ? Errors : default;
        }

        /// <summary>
        ///     解构 Result
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="isFailed"></param>
        /// <param name="value"></param>
        public void Deconstruct(out bool isSuccess, out bool isFailed, out TValue value)
        {
            isSuccess = IsSuccess;
            isFailed = IsFailed;
            value = IsSuccess ? Value : default;
        }

        /// <summary>
        ///     解构 Result
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="isFailed"></param>
        /// <param name="value"></param>
        /// <param name="errors"></param>
        public void Deconstruct(out bool isSuccess, out bool isFailed, out TValue value, out List<IError> errors)
        {
            isSuccess = IsSuccess;
            isFailed = IsFailed;
            value = IsSuccess ? Value : default;
            errors = IsFailed ? Errors.ToList() : default;
        }

        private void ThrowIfValueUnavailable()
        {
            if (IsFailed)
                throw new InvalidOperationException(
                    $"Result is in status failed. Value is not set. Having: {ReasonFormat.ErrorReasonsToString(Errors)}");
        }
    }
}