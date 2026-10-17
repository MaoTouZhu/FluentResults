using System;
using System.Threading.Tasks;

namespace FluentResults.Extensions
{
    /// <summary>
    /// Result 的扩展方法
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// 通过 errorMapper 映射结果中的所有错误
        /// </summary>
        /// <param name="resultTask">当前结果</param>
        /// <param name="errorMapper">用于转换错误的函数</param>
        public static async Task<Result> MapErrors(this Task<Result> resultTask, Func<IError, IError> errorMapper)
        {
            var result = await resultTask;
            return result.MapErrors(errorMapper);
        }

        /// <summary>
        /// 通过 errorMapper 映射结果中的所有错误
        /// </summary>
        /// <param name="resultTask">当前结果</param>
        /// <param name="errorMapper">用于转换错误的函数</param>
        public static async ValueTask<Result> MapErrors(this ValueTask<Result> resultTask, Func<IError, IError> errorMapper)
        {
            var result = await resultTask;
            return result.MapErrors(errorMapper);
        }

        /// <summary>
        /// 通过 errorMapper 映射结果中的所有错误
        /// </summary>
        /// <param name="resultTask">当前结果</param>
        /// <param name="errorMapper">用于转换错误的函数</param>
        public static async Task<Result<T>> MapErrors<T>(this Task<Result<T>> resultTask, Func<IError, IError> errorMapper)
        {
            var result = await resultTask;
            return result.MapErrors(errorMapper);
        }

        /// <summary>
        /// 通过 errorMapper 映射结果中的所有错误
        /// </summary>
        /// <param name="resultTask">当前结果</param>
        /// <param name="errorMapper">用于转换错误的函数</param>
        public static async ValueTask<Result<T>> MapErrors<T>(this ValueTask<Result<T>> resultTask, Func<IError, IError> errorMapper)
        {
            var result = await resultTask;
            return result.MapErrors(errorMapper);
        }

        /// <summary>
        /// 通过 successMapper 映射结果中的所有成功
        /// </summary>
        /// <param name="resultTask">当前结果</param>
        /// <param name="errorMapper">用于转换成功项的函数</param>
        public static async Task<Result> MapSuccesses(this Task<Result> resultTask, Func<ISuccess, ISuccess> errorMapper)
        {
            var result = await resultTask;
            return result.MapSuccesses(errorMapper);
        }

        /// <summary>
        /// 通过 successMapper 映射结果中的所有成功
        /// </summary>
        /// <param name="resultTask">当前结果</param>
        /// <param name="errorMapper">用于转换成功项的函数</param>
        public static async ValueTask<Result> MapSuccesses(this ValueTask<Result> resultTask, Func<ISuccess, ISuccess> errorMapper)
        {
            var result = await resultTask;
            return result.MapSuccesses(errorMapper);
        }

        /// <summary>
        /// 通过 successMapper 映射结果中的所有成功
        /// <param name="resultTask">当前结果</param>
        /// <param name="errorMapper">用于转换成功项的函数</param>
        /// </summary>
        public static async Task<Result<T>> MapSuccesses<T>(this Task<Result<T>> resultTask, Func<ISuccess, ISuccess> errorMapper)
        {
            var result = await resultTask;
            return result.MapSuccesses(errorMapper);
        }

        /// <summary>
        /// 通过 successMapper 映射结果中的所有成功
        /// </summary>
        /// <param name="resultTask">当前结果</param>
        /// <param name="errorMapper">用于转换成功项的函数</param>
        public static async ValueTask<Result<T>> MapSuccesses<T>(this ValueTask<Result<T>> resultTask, Func<ISuccess, ISuccess> errorMapper)
        {
            var result = await resultTask;
            return result.MapSuccesses(errorMapper);
        }

        /// <summary>
        /// 将带值的结果异步转换为带有另一个值的结果，该转换可能失败。
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="resultTask">当前结果</param>
        /// <param name="bind">可能失败的转换。</param>
        public static async Task<Result<TNew>> Bind<TOld, TNew>(this Task<Result<TOld>> resultTask, Func<TOld, Task<Result<TNew>>> bind)
        {
            var result = await resultTask;
            return await result.Bind(bind);
        }

        /// <summary>
        /// 将带值的结果异步转换为带有另一个值的结果，该转换可能失败。
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="resultTask">当前结果</param>
        /// <param name="bind">可能失败的转换。</param>
        public static async ValueTask<Result<TNew>> Bind<TOld, TNew>(this ValueTask<Result<TOld>> resultTask, Func<TOld, ValueTask<Result<TNew>>> bind)
        {
            var result = await resultTask;
            return await result.Bind(bind);
        }

        /// <summary>
        /// 将带值的结果异步转换为带有另一个值的结果，该转换可能失败。
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="resultTask">当前结果</param>
        /// <param name="bind">可能失败的转换。</param>
        public static async Task<Result<TNew>> Bind<TOld, TNew>(this Task<Result<TOld>> resultTask, Func<TOld, Result<TNew>> bind)
        {
            var result = await resultTask;
            return result.Bind(bind);
        }
        /// <summary>
        /// 将带值的结果异步转换为带有另一个值的结果，该转换可能失败。
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="resultTask">当前结果</param>
        /// <param name="bind">可能失败的转换。</param>

        public static async ValueTask<Result<TNew>> Bind<TOld, TNew>(this ValueTask<Result<TOld>> resultTask, Func<TOld, Result<TNew>> bind)
        {
            var result = await resultTask;
            return result.Bind(bind);
        }

        /// <summary>
        /// 将带值的结果异步转换为带有另一个值的结果，该转换可能失败。
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="resultTask">当前结果</param>
        /// <param name="bind">可能失败的转换。</param>
        public static async Task<Result> Bind<TOld>(this Task<Result<TOld>> resultTask, Func<TOld, Task<Result>> bind)
        {
            var result = await resultTask;
            return await result.Bind(bind);
        }

        /// <summary>
        /// 将带值的结果异步转换为带有另一个值的结果，该转换可能失败。
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="resultTask">当前结果</param>
        /// <param name="bind">可能失败的转换。</param>
        public static async Task<Result> Bind<TOld>(this Task<Result<TOld>> resultTask, Func<TOld, Result> bind)
        {
            var result = await resultTask;
            return result.Bind(bind);
        }

        /// <summary>
        /// 将带值的结果异步转换为带有另一个值的结果，该转换可能失败。
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="resultTask">当前结果</param>
        /// <param name="bind">可能失败的转换。</param>
        public static async ValueTask<Result> Bind<TOld>(this ValueTask<Result<TOld>> resultTask, Func<TOld, Result> bind)
        {
            var result = await resultTask;
            return result.Bind(bind);
        }

        /// <summary>
        /// 将带值的结果异步转换为带有另一个值的结果，该转换可能失败。
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="resultTask">当前结果</param>
        /// <param name="bind">可能失败的转换。</param>
        public static async ValueTask<Result> Bind<TOld>(this ValueTask<Result<TOld>> resultTask, Func<TOld, ValueTask<Result>> bind)
        {
            var result = await resultTask;
            return await result.Bind(bind);
        }

        /// <summary>
        /// 将带值的结果异步转换为带有另一个值的结果，该转换可能失败。
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="resultTask">当前结果</param>
        /// <param name="bind">可能失败的转换。</param>
        public static async Task<Result<TNew>> Bind<TNew>(this Task<Result> resultTask, Func<Task<Result<TNew>>> bind)
        {
            var result = await resultTask;
            return await result.Bind(bind);
        }

        /// <summary>
        /// 将带值的结果异步转换为带有另一个值的结果，该转换可能失败。
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="resultTask">当前结果</param>
        /// <param name="bind">可能失败的转换。</param>
        public static async ValueTask<Result<TNew>> Bind<TNew>(this ValueTask<Result> resultTask, Func<ValueTask<Result<TNew>>> bind)
        {
            var result = await resultTask;
            return await result.Bind(bind);
        }

        /// <summary>
        /// 将带值的结果异步转换为带有另一个值的结果，该转换可能失败。
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="resultTask">当前结果</param>
        /// <param name="bind">可能失败的转换。</param>
        public static async Task<Result> Bind(this Task<Result> resultTask, Func<Task<Result>> bind)
        {
            var result = await resultTask;
            return await result.Bind(bind);
        }

        /// <summary>
        /// 将带值的结果异步转换为带有另一个值的结果，该转换可能失败。
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="resultTask">当前结果</param>
        /// <param name="bind">可能失败的转换。</param>
        public static async ValueTask<Result> Bind(this ValueTask<Result> resultTask, Func<ValueTask<Result>> bind)
        {
            var result = await resultTask;
            return await result.Bind(bind);
        }

        /// <summary>
        /// 将带值的结果转换为带有另一个值的结果。使用 valueConverter 参数指定值的转换逻辑。
        /// </summary>
        public static async Task<Result<TNewValue>> Map<TOldValue, TNewValue>(this Task<Result<TOldValue>> resultTask, Func<TOldValue, TNewValue> valueConverter)
        {
            var result = await resultTask;
            return result.Map(valueConverter);
        }

        /// <summary>
        /// 将带值的结果转换为带有另一个值的结果。使用 valueConverter 参数指定值的转换逻辑。
        /// </summary>
        public static async Task<Result<TNewValue>> Map<TOldValue, TNewValue>(this ValueTask<Result<TOldValue>> resultTask, Func<TOldValue, TNewValue> valueConverter)
        {
            var result = await resultTask;
            return result.Map(valueConverter);
        }

        /// <summary>
        /// 将不含值的结果转换为包含值的结果
        /// </summary>
        /// <typeparam name="TValue">值的类型</typeparam>
        /// <param name="resultTask">当前结果</param>
        /// <param name="value">要添加到新结果中的值</param>
        public static async Task<Result<TValue>> ToResult<TValue>(this Task<Result> resultTask, TValue value)
        {
            var result = await resultTask;
            return result.ToResult(value);
        }

        /// <summary>
        /// 将不含值的结果转换为包含值的结果
        /// </summary>
        /// <typeparam name="TValue">值的类型</typeparam>
        /// <param name="resultTask">当前结果</param>
        /// <param name="value">要添加到新结果中的值</param>
        public static async Task<Result<TValue>> ToResult<TValue>(this ValueTask<Result> resultTask, TValue value)
        {
            var result = await resultTask;
            return result.ToResult(value);
        }
        
        /// <summary>
        /// 根据参数 isFailure 创建成功或失败的结果
        /// </summary>
        /// <param name="source">上一个结果</param>
        /// <param name="isFailure">用于检查结果是否应失败的条件</param>
        /// <param name="error">错误消息</param>
        /// <returns>如果上一个结果已失败则返回它，否则返回一个新结果</returns>
        public static Result OrFailIf(this Result source, bool isFailure, string error)
        {
            if (source.IsFailed)
            {
                return source;
            }
            
            return isFailure ? Result.Fail(error) : source;
        }
    }
}
