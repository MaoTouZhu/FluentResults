using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// IEnumerable 的实用扩展方法
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// 将多个结果对象合并为一个结果
        /// </summary>
        public static Result Merge(this IEnumerable<Result> results)
        {
            return ResultHelper.Merge(results);
        }

        /// <summary>
        /// 将多个结果对象合并为一个结果
        /// </summary>
        public static Result<IEnumerable<TValue>> Merge<TValue>(this IEnumerable<Result<TValue>> results)
        {
            return ResultHelper.MergeWithValue(results);
        }
    }
}