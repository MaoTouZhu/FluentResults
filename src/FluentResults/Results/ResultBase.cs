using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// ResultBase 的定义
    /// </summary>
    public interface IResultBase
    {
        /// <summary>
        /// 如果 Reasons 包含至少一个错误，则为 true
        /// </summary>
        bool IsFailed { get; }

        /// <summary>
        /// 如果 Reasons 不包含任何错误，则为 true
        /// </summary>
        bool IsSuccess { get; }

        /// <summary>
        /// 获取所有原因（错误和成功）
        /// </summary>
        List<IReason> Reasons { get; }

        /// <summary>
        /// 获取所有错误
        /// </summary>
        IReadOnlyList<IError> Errors { get; }

        /// <summary>
        /// 获取所有成功
        /// </summary>
        IReadOnlyList<ISuccess> Successes { get; }
    }

    /// <summary>
    /// <see cref="IResultBase"/> 的默认实现
    /// </summary>
    public abstract class ResultBase : IResultBase
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsFailed => Reasons.OfType<IError>().Any();

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsSuccess => !IsFailed;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public List<IReason> Reasons { get; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IReadOnlyList<IError> Errors => Reasons.OfType<IError>().ToList().AsReadOnly();

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IReadOnlyList<ISuccess> Successes => Reasons.OfType<ISuccess>().ToList().AsReadOnly();

        /// <summary>
        /// 所有错误消息按当前平台的换行符（<see cref="Environment.NewLine"/>）拼接；如果结果没有错误，则为 null
        /// </summary>
        public string ErrorMessage => ResultHelper.GetErrorMessage(this);

        /// <summary>
        /// 默认构造函数
        /// </summary>
        protected ResultBase()
        {
            Reasons = new List<IReason>();
        }

        /// <summary>
        /// 检查结果对象是否包含特定类型的错误
        /// </summary>
        public bool HasError<TError>() where TError : IError
        {
            return HasError<TError>(out _);
        }

        /// <summary>
        /// 检查结果对象是否包含特定类型的错误
        /// </summary>
        public bool HasError<TError>(out IEnumerable<TError> result) where TError : IError
        {
            return HasError<TError>(e => true, out result);
        }

        /// <summary>
        /// 检查结果对象是否包含特定类型且满足特定条件的错误
        /// </summary>
        public bool HasError<TError>(Func<TError, bool> predicate) where TError : IError
        {
            return HasError<TError>(predicate, out _);
        }

        /// <summary>
        /// 检查结果对象是否包含特定类型且满足特定条件的错误
        /// </summary>
        public bool HasError<TError>(Func<TError, bool> predicate, out IEnumerable<TError> result) where TError : IError
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return ResultHelper.HasError(Errors, predicate, out result);
        }

        /// <summary>
        /// 检查结果对象是否包含满足特定条件的错误
        /// </summary>
        public bool HasError(Func<IError, bool> predicate)
        {
            return HasError(predicate, out _);
        }

        /// <summary>
        /// 检查结果对象是否包含满足特定条件的错误
        /// </summary>
        public bool HasError(Func<IError, bool> predicate, out IEnumerable<IError> result)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return ResultHelper.HasError(Errors, predicate, out result);
        }

        /// <summary>
        /// 检查结果对象是否包含特定类型的异常
        /// </summary>
        public bool HasException<TException>() where TException : Exception
        {
            return HasException<TException>(out _);
        }

        /// <summary>
        /// 检查结果对象是否包含特定类型的异常
        /// </summary>
        public bool HasException<TException>(out IEnumerable<IError> result) where TException : Exception
        {
            return HasException<TException>(error => true, out result);
        }

        /// <summary>
        /// 检查结果对象是否包含特定类型且满足特定条件的异常
        /// </summary>
        public bool HasException<TException>(Func<TException, bool> predicate) where TException : Exception
        {
            return HasException(predicate, out _);
        }

        /// <summary>
        /// 检查结果对象是否包含特定类型且满足特定条件的异常
        /// </summary>
        public bool HasException<TException>(Func<TException, bool> predicate, out IEnumerable<IError> result) where TException : Exception
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return ResultHelper.HasException(Errors, predicate, out result);
        }

        

        /// <summary>
        /// 检查结果对象是否包含特定类型的成功
        /// </summary>
        public bool HasSuccess<TSuccess>() where TSuccess : ISuccess
        {
            return HasSuccess<TSuccess>(success => true, out _);
        }

        /// <summary>
        /// 检查结果对象是否包含特定类型的成功
        /// </summary>
        public bool HasSuccess<TSuccess>(out IEnumerable<TSuccess> result) where TSuccess : ISuccess
        {
            return HasSuccess<TSuccess>(success => true, out result);
        }

        /// <summary>
        /// 检查结果对象是否包含特定类型且满足特定条件的成功
        /// </summary>
        public bool HasSuccess<TSuccess>(Func<TSuccess, bool> predicate) where TSuccess : ISuccess
        {
            return HasSuccess(predicate, out _);
        }

        /// <summary>
        /// 检查结果对象是否包含特定类型且满足特定条件的成功
        /// </summary>
        public bool HasSuccess<TSuccess>(Func<TSuccess, bool> predicate, out IEnumerable<TSuccess> result) where TSuccess : ISuccess
        {
            return ResultHelper.HasSuccess(Successes, predicate, out result);
        }

        /// <summary>
        /// 检查结果对象是否包含满足特定条件的成功
        /// </summary>
        public bool HasSuccess(Func<ISuccess, bool> predicate, out IEnumerable<ISuccess> result)
        {
            return ResultHelper.HasSuccess(Successes, predicate, out result);
        }

        /// <summary>
        /// 检查结果对象是否包含满足特定条件的成功
        /// </summary>
        public bool HasSuccess(Func<ISuccess, bool> predicate)
        {
            return ResultHelper.HasSuccess(Successes, predicate, out _);
        }
    }

    /// <summary>
    /// <see cref="IResultBase"/> 泛型的默认实现
    /// </summary>
    public abstract class ResultBase<TResult> : ResultBase
        where TResult : ResultBase<TResult>

    {
        /// <summary>
        /// 添加一个原因（成功或错误）
        /// </summary>
        public TResult WithReason(IReason reason)
        {
            Reasons.Add(reason);
            return (TResult)this;
        }

        /// <summary>
        /// 添加多个原因（成功或错误）
        /// </summary>
        public TResult WithReasons(IEnumerable<IReason> reasons)
        {
            Reasons.AddRange(reasons);
            return (TResult)this;
        }

        /// <summary>
        /// 添加一个错误
        /// </summary>
        public TResult WithError(string errorMessage)
        {
            return WithError(Result.Settings.ErrorFactory(errorMessage));
        }

        /// <summary>
        /// 添加一个错误
        /// </summary>
        public TResult WithError(IError error)
        {
            return WithReason(error);
        }

        /// <summary>
        /// 添加多个错误
        /// </summary>
        public TResult WithErrors(IEnumerable<IError> errors)
        {
            return WithReasons(errors);
        }

        /// <summary>
        /// 添加多个错误
        /// </summary>
        public TResult WithErrors(IEnumerable<string> errors)
        {
            return WithReasons(errors.Select(errorMessage => Result.Settings.ErrorFactory(errorMessage)));
        }

        /// <summary>
        /// 添加一个错误
        /// </summary>
        public TResult WithError<TError>()
            where TError : IError, new()
        {
            return WithError(new TError());
        }

        /// <summary>
        /// 添加一个成功
        /// </summary>
        public TResult WithSuccess(string successMessage)
        {
            return WithSuccess(Result.Settings.SuccessFactory(successMessage));
        }

        /// <summary>
        /// 添加一个成功
        /// </summary>
        public TResult WithSuccess(ISuccess success)
        {
            return WithReason(success);
        }

        /// <summary>
        /// 添加一个成功
        /// </summary>
        public TResult WithSuccess<TSuccess>()
            where TSuccess : Success, new()
        {
            return WithSuccess(new TSuccess());
        }

        /// <summary>
        /// 添加多个成功
        /// </summary>
        public TResult WithSuccesses(IEnumerable<ISuccess> successes)
        {
            foreach (var success in successes)
            {
                WithSuccess(success);
            }

            return (TResult)this;
        }

        /// <summary>
        /// 记录结果日志。通过 Result.Setup(..) 配置日志记录器
        /// </summary>
        public TResult Log(LogLevel logLevel = LogLevel.Information)
        {
            return Log(string.Empty, null, logLevel);
        }

        /// <summary>
        /// 记录结果日志。通过 Result.Setup(..) 配置日志记录器
        /// </summary>
        public TResult Log(string context, LogLevel logLevel = LogLevel.Information)
        {
            return Log(context, null, logLevel);
        }

        /// <summary>
        /// 使用特定日志上下文记录结果日志。通过 Result.Setup(..) 配置日志记录器
        /// </summary>
        public TResult Log(string context, string content, LogLevel logLevel = LogLevel.Information)
        {
            var logger = Result.Settings.Logger;

            logger.Log(context, content, this, logLevel);

            return (TResult)this;
        }

        /// <summary>
        /// 使用类型化上下文记录结果日志。通过 Result.Setup(..) 配置日志记录器
        /// </summary>
        public TResult Log<TContext>(LogLevel logLevel = LogLevel.Information)
        {
            return Log<TContext>(null, logLevel);
        }

        /// <summary>
        /// 使用类型化上下文记录结果日志。通过 Result.Setup(..) 配置日志记录器
        /// </summary>
        public TResult Log<TContext>(string content, LogLevel logLevel = LogLevel.Information)
        {
            var logger = Result.Settings.Logger;

            logger.Log<TContext>(content, this, logLevel);

            return (TResult)this;
        }

        /// <summary>
        /// 仅当结果成功时记录结果日志。通过 Result.Setup(..) 配置日志记录器
        /// </summary>
        public TResult LogIfSuccess(LogLevel logLevel = LogLevel.Information)
        {
            if (IsSuccess)
                return Log(logLevel);

            return (TResult)this;
        }

        /// <summary>
        /// 仅当结果成功时，使用特定日志上下文记录结果日志。通过 Result.Setup(..) 配置日志记录器
        /// </summary>
        public TResult LogIfSuccess(string context, string content = null, LogLevel logLevel = LogLevel.Information)
        {
            if (IsSuccess)
                return Log(context, content, logLevel);

            return (TResult)this;
        }

        /// <summary>
        /// 仅当结果成功时，使用类型化上下文记录结果日志。通过 Result.Setup(..) 配置日志记录器
        /// </summary>
        public TResult LogIfSuccess<TContext>(string content = null, LogLevel logLevel = LogLevel.Information)
        {
            if (IsSuccess)
                return Log<TContext>(content, logLevel);

            return (TResult)this;
        }

        /// <summary>
        /// 仅当结果失败时记录结果日志。通过 Result.Setup(..) 配置日志记录器
        /// </summary>
        public TResult LogIfFailed(LogLevel logLevel = LogLevel.Error)
        {
            if (IsFailed)
                return Log(logLevel);

            return (TResult)this;
        }

        /// <summary>
        /// 仅当结果失败时，使用特定日志上下文记录结果日志。通过 Result.Setup(..) 配置日志记录器
        /// </summary>
        public TResult LogIfFailed(string context, string content = null, LogLevel logLevel = LogLevel.Error)
        {
            if (IsFailed)
                return Log(context, content, logLevel);

            return (TResult)this;
        }

        /// <summary>
        /// 仅当结果失败时，使用类型化上下文记录结果日志。通过 Result.Setup(..) 配置日志记录器
        /// </summary>
        public TResult LogIfFailed<TContext>(string content = null, LogLevel logLevel = LogLevel.Error)
        {
            if (IsFailed)
                return Log<TContext>(content, logLevel);

            return (TResult)this;
        }

        /// <summary>
        /// 抛出此结果所表示的异常。
        /// 第一个 <see cref="IExceptionalError"/> 保留其原始异常；否则会将第一个
        /// 错误消息包装在 <see cref="Exception"/> 中；成功的结果会抛出
        /// <see cref="Result.DefaultError"/>。
        /// </summary>
        public void Throw()
        {
            throw ResultHelper.ToException(this);
        }

        /// <summary>
        /// 仅当结果失败时抛出。参见 <see cref="Throw"/>。
        /// </summary>
        public void ThrowIfFailed()
        {
            if (IsFailed)
                Throw();
        }

        /// <summary>
        /// 重写 ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var reasonsString = Reasons.Any()
                                    ? $", Reasons='{ReasonFormat.ReasonsToString(Reasons)}'"
                                    : string.Empty;

            return $"Result: IsSuccess='{IsSuccess}'{reasonsString}";
        }
    }
}