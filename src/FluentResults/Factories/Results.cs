using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public partial class Result
    {
        static Result()
        {
            Settings = new ResultSettingsBuilder().Build();
        }

        internal static ResultSettings Settings { get; private set; }

        /// <summary>
        ///     设置全局设置，例如日志
        /// </summary>
        public static void Setup(Action<ResultSettingsBuilder> setupFunc)
        {
            var settingsBuilder = new ResultSettingsBuilder();
            setupFunc(settingsBuilder);

            Settings = settingsBuilder.Build();
        }

        /// <summary>
        ///     创建一个成功的结果
        /// </summary>
        public static Result Ok()
        {
            return new Result();
        }

        /// <summary>
        ///     使用给定的错误创建一个失败的结果
        /// </summary>
        public static Result Fail(IError error)
        {
            var result = new Result();
            result.WithError(error);
            return result;
        }

        /// <summary>
        ///     使用给定的错误消息创建一个失败的结果。内部会从错误工厂创建一个错误对象。
        /// </summary>
        public static Result Fail(string errorMessage)
        {
            var result = new Result();
            result.WithError(Settings.ErrorFactory(errorMessage));
            return result;
        }

        /// <summary>
        ///     使用给定的错误消息创建一个失败的结果。内部会从错误工厂
        ///     创建一个错误对象列表。
        /// </summary>
        public static Result Fail(IEnumerable<string> errorMessages)
        {
            if (errorMessages == null)
                throw new ArgumentNullException(nameof(errorMessages), "The list of error messages cannot be null");

            if (!errorMessages.Any())
                throw new ArgumentException("The list of errors is empty", nameof(errorMessages));

            var result = new Result();
            result.WithErrors(errorMessages.Select(Settings.ErrorFactory));
            return result;
        }

        /// <summary>
        ///     使用给定的错误创建一个失败的结果。
        /// </summary>
        public static Result Fail(IEnumerable<IError> errors)
        {
            if (errors == null)
                throw new ArgumentNullException(nameof(errors), "The list of errors cannot be null");

            if (!errors.Any())
                throw new ArgumentException("The list of errors is empty", nameof(errors));


            var result = new Result();
            result.WithErrors(errors);
            return result;
        }

        /// <summary>
        ///     使用给定的值创建一个成功的结果
        /// </summary>
        public static Result<TValue> Ok<TValue>(TValue value)
        {
            var result = new Result<TValue>();
            result.WithValue(value);
            return result;
        }

        /// <summary>
        ///     使用给定的错误创建一个失败的结果
        /// </summary>
        public static Result<TValue> Fail<TValue>(IError error)
        {
            var result = new Result<TValue>();
            result.WithError(error);
            return result;
        }

        /// <summary>
        ///     使用给定的错误消息创建一个失败的结果。内部会从错误工厂创建一个错误对象。
        /// </summary>
        public static Result<TValue> Fail<TValue>(string errorMessage)
        {
            var result = new Result<TValue>();
            result.WithError(Settings.ErrorFactory(errorMessage));
            return result;
        }

        /// <summary>
        ///     使用给定的错误消息创建一个失败的结果。内部会从错误工厂
        ///     创建一个错误对象列表。
        /// </summary>
        public static Result<TValue> Fail<TValue>(IEnumerable<string> errorMessages)
        {
            if (errorMessages == null)
                throw new ArgumentNullException(nameof(errorMessages), "The list of error messages cannot be null");

            if (!errorMessages.Any())
                throw new ArgumentException("The list of errors is empty", nameof(errorMessages));

            var result = new Result<TValue>();
            result.WithErrors(errorMessages.Select(Settings.ErrorFactory));
            return result;
        }

        /// <summary>
        ///     使用给定的错误创建一个失败的结果。
        /// </summary>
        public static Result<TValue> Fail<TValue>(IEnumerable<IError> errors)
        {
            if (errors == null)
                throw new ArgumentNullException(nameof(errors), "The list of errors cannot be null");

            if (!errors.Any())
                throw new ArgumentException("The list of errors is empty", nameof(errors));

            var result = new Result<TValue>();
            result.WithErrors(errors);
            return result;
        }

        /// <summary>
        ///     将多个结果对象合并为一个结果对象
        /// </summary>
        public static Result Merge(params ResultBase[] results)
        {
            return ResultHelper.Merge(results);
        }

        /// <summary>
        ///     将多个结果对象合并为一个结果对象。返回一个包含合并后值列表的结果。
        /// </summary>
        public static Result<IEnumerable<TValue>> Merge<TValue>(params Result<TValue>[] results)
        {
            return ResultHelper.MergeWithValue(results);
        }

        /// <summary>
        ///     将多个结果对象合并为一个结果对象。返回一个包含展平后的合并
        ///     值列表的结果。
        /// </summary>
        public static Result<IEnumerable<TValue>> MergeFlat<TValue, TArray>(params Result<TArray>[] results)
            where TArray : IEnumerable<TValue>
        {
            return ResultHelper.MergeWithValue<TValue, TArray>(results);
        }

        /// <summary>
        ///     根据参数 isSuccess 创建成功或失败的结果
        /// </summary>
        public static Result OkIf(bool isSuccess, IError error)
        {
            return isSuccess ? Ok() : Fail(error);
        }

        /// <summary>
        ///     根据参数 isSuccess 创建成功或失败的结果
        /// </summary>
        public static Result OkIf(bool isSuccess, string error)
        {
            return isSuccess ? Ok() : Fail(error);
        }

        /// <summary>
        ///     根据参数 isSuccess 创建成功或失败的结果
        /// </summary>
        /// <remarks>
        ///     错误采用延迟求值。
        /// </remarks>
        public static Result OkIf(bool isSuccess, Func<IError> errorFactory)
        {
            return isSuccess ? Ok() : Fail(errorFactory.Invoke());
        }

        /// <summary>
        ///     根据参数 isSuccess 创建成功或失败的结果
        /// </summary>
        /// <remarks>
        ///     错误采用延迟求值。
        /// </remarks>
        public static Result OkIf(bool isSuccess, Func<string> errorMessageFactory)
        {
            return isSuccess ? Ok() : Fail(errorMessageFactory.Invoke());
        }

        /// <summary>
        ///     根据参数 isFailure 创建成功或失败的结果
        /// </summary>
        public static Result FailIf(bool isFailure, IError error)
        {
            return isFailure ? Fail(error) : Ok();
        }

        /// <summary>
        ///     根据参数 isFailure 创建成功或失败的结果
        /// </summary>
        public static Result FailIf(bool isFailure, string error)
        {
            return isFailure ? Fail(error) : Ok();
        }

        /// <summary>
        ///     根据参数 isFailure 创建成功或失败的结果
        /// </summary>
        /// <remarks>
        ///     错误采用延迟求值。
        /// </remarks>
        public static Result FailIf(bool isFailure, Func<IError> errorFactory)
        {
            return isFailure ? Fail(errorFactory.Invoke()) : Ok();
        }

        /// <summary>
        ///     根据参数 isFailure 创建成功或失败的结果
        /// </summary>
        /// <remarks>
        ///     错误采用延迟求值。
        /// </remarks>
        public static Result FailIf(bool isFailure, Func<string> errorMessageFactory)
        {
            return isFailure ? Fail(errorMessageFactory.Invoke()) : Ok();
        }

        /// <summary>
        ///     根据参数 isFailure 创建成功或失败的结果，并包含指定的错误
        /// </summary>
        public static Result FailIf(bool isFailure, IEnumerable<IError> errors)
        {
            return isFailure ? Fail(errors) : Ok();
        }

        /// <summary>
        ///     如果存在任何错误对象，则创建成功或失败的结果
        /// </summary>
        public static Result FailIfNotEmpty(IEnumerable<IError> errors)
        {
            return errors.Any() ? Fail(errors) : Ok();
        }

        /// <summary>
        ///     根据是否存在任何错误对象创建成功或失败的结果
        /// </summary>
        /// <remarks>
        ///     错误采用延迟求值。
        /// </remarks>
        public static Result FailIfNotEmpty<T>(IEnumerable<T> errors, Func<T, IError> func)
        {
            return errors.Any() ? Fail(errors.Select(error => func(error))) : Ok();
        }

        /// <summary>
        ///     执行该操作。如果操作中抛出异常，则该异常会通过
        ///     catchHandler 转换为 Error 对象
        /// </summary>
        public static Result Try(Action action, Func<Exception, IError> catchHandler = null)
        {
            catchHandler = catchHandler ?? Settings.DefaultTryCatchHandler;

            try
            {
                action();
                return Ok();
            }
            catch (Exception e)
            {
                return Fail(catchHandler(e));
            }
        }

        /// <summary>
        ///     执行该操作。如果操作中抛出异常，则该异常会通过
        ///     catchHandler 转换为 Error 对象
        /// </summary>
        public static async Task<Result> Try(Func<Task> action, Func<Exception, IError> catchHandler = null)
        {
            catchHandler = catchHandler ?? Settings.DefaultTryCatchHandler;

            try
            {
                await action();
                return Ok();
            }
            catch (Exception e)
            {
                return Fail(catchHandler(e));
            }
        }

        /// <summary>
        ///     执行该操作。如果操作中抛出异常，则该异常会通过
        ///     catchHandler 转换为 Error 对象
        /// </summary>
        public static async ValueTask<Result> Try(Func<ValueTask> action, Func<Exception, IError> catchHandler = null)
        {
            catchHandler = catchHandler ?? Settings.DefaultTryCatchHandler;

            try
            {
                await action();
                return Ok();
            }
            catch (Exception e)
            {
                return Fail(catchHandler(e));
            }
        }

        /// <summary>
        ///     执行该操作。如果操作中抛出异常，则该异常会通过
        ///     catchHandler 转换为 Error 对象
        /// </summary>
        public static Result<T> Try<T>(Func<T> action, Func<Exception, IError> catchHandler = null)
        {
            catchHandler = catchHandler ?? Settings.DefaultTryCatchHandler;

            try
            {
                return Ok(action());
            }
            catch (Exception e)
            {
                return Fail(catchHandler(e));
            }
        }

        /// <summary>
        ///     执行该操作。如果操作中抛出异常，则该异常会通过
        ///     catchHandler 转换为 Error 对象
        /// </summary>
        public static async Task<Result<T>> Try<T>(Func<Task<T>> action, Func<Exception, IError> catchHandler = null)
        {
            catchHandler = catchHandler ?? Settings.DefaultTryCatchHandler;

            try
            {
                return Ok(await action());
            }
            catch (Exception e)
            {
                return Fail(catchHandler(e));
            }
        }

        /// <summary>
        ///     执行该操作。如果操作中抛出异常，则该异常会通过
        ///     catchHandler 转换为 Error 对象
        /// </summary>
        public static async ValueTask<Result<T>> Try<T>(Func<ValueTask<T>> action,
            Func<Exception, IError> catchHandler = null)
        {
            catchHandler = catchHandler ?? Settings.DefaultTryCatchHandler;

            try
            {
                return Ok(await action());
            }
            catch (Exception e)
            {
                return Fail(catchHandler(e));
            }
        }

        /// <summary>
        ///     执行该操作。如果操作中抛出异常，则该异常会通过
        ///     catchHandler 转换为 Error 对象
        /// </summary>
        public static Result Try(Func<Result> action, Func<Exception, IError> catchHandler = null)
        {
            catchHandler = catchHandler ?? Settings.DefaultTryCatchHandler;

            try
            {
                return action();
            }
            catch (Exception e)
            {
                return Fail(catchHandler(e));
            }
        }

        /// <summary>
        ///     执行该操作。如果操作中抛出异常，则该异常会通过
        ///     catchHandler 转换为 Error 对象
        /// </summary>
        public static async Task<Result> Try(Func<Task<Result>> action, Func<Exception, IError> catchHandler = null)
        {
            catchHandler = catchHandler ?? Settings.DefaultTryCatchHandler;

            try
            {
                return await action();
            }
            catch (Exception e)
            {
                return Fail(catchHandler(e));
            }
        }

        /// <summary>
        ///     执行该操作。如果操作中抛出异常，则该异常会通过
        ///     catchHandler 转换为 Error 对象
        /// </summary>
        public static async ValueTask<Result> Try(Func<ValueTask<Result>> action,
            Func<Exception, IError> catchHandler = null)
        {
            catchHandler = catchHandler ?? Settings.DefaultTryCatchHandler;

            try
            {
                return await action();
            }
            catch (Exception e)
            {
                return Fail(catchHandler(e));
            }
        }

        /// <summary>
        ///     执行该操作。如果操作中抛出异常，则该异常会通过
        ///     catchHandler 转换为 Error 对象
        /// </summary>
        public static Result<T> Try<T>(Func<Result<T>> action, Func<Exception, IError> catchHandler = null)
        {
            catchHandler = catchHandler ?? Settings.DefaultTryCatchHandler;

            try
            {
                return action();
            }
            catch (Exception e)
            {
                return Fail(catchHandler(e));
            }
        }

        /// <summary>
        ///     执行该操作。如果操作中抛出异常，则该异常会通过
        ///     catchHandler 转换为 Error 对象
        /// </summary>
        public static async Task<Result<T>> Try<T>(Func<Task<Result<T>>> action,
            Func<Exception, IError> catchHandler = null)
        {
            catchHandler = catchHandler ?? Settings.DefaultTryCatchHandler;

            try
            {
                return await action();
            }
            catch (Exception e)
            {
                return Fail(catchHandler(e));
            }
        }

        /// <summary>
        ///     执行该操作。如果操作中抛出异常，则该异常会通过
        ///     catchHandler 转换为 Error 对象
        /// </summary>
        public static async ValueTask<Result<T>> Try<T>(Func<ValueTask<Result<T>>> action,
            Func<Exception, IError> catchHandler = null)
        {
            catchHandler = catchHandler ?? Settings.DefaultTryCatchHandler;

            try
            {
                return await action();
            }
            catch (Exception e)
            {
                return Fail(catchHandler(e));
            }
        }
    }
}