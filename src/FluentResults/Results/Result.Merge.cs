using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public partial class Result
    {
        /// <summary>
        /// 将当前结果与其它结果合并为一个结果对象
        /// </summary>
        /// <param name="results">要合并的其它结果</param>
        public Result Merge(params IResultBase[] results)
            => Result.Merge(new ResultBase[] { this }.Concat(results.OfType<ResultBase>()).ToArray());
    }

    public partial class Result<TValue>
    {
        /// <summary>
        /// 将当前结果与其它结果合并为一个包含值列表的结果对象
        /// </summary>
        /// <param name="results">要合并的其它结果</param>
        public Result<IEnumerable<TValue>> Merge(params Result<TValue>[] results)
            => Result.Merge(new[] { this }.Concat(results).ToArray());
    }
}
