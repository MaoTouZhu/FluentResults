using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public partial class Result
    {
        /// <summary>
        /// Merges this result with the given results into one result
        /// </summary>
        /// <param name="results">The results to merge with this result</param>
        public Result Merge(params IResultBase[] results)
            => Result.Merge(new ResultBase[] { this }.Concat(results.OfType<ResultBase>()).ToArray());
    }

    public partial class Result<TValue>
    {
        /// <summary>
        /// Merges this result with the given results into a result containing the merged value list
        /// </summary>
        /// <param name="results">The results to merge with this result</param>
        public Result<IEnumerable<TValue>> Merge(params Result<TValue>[] results)
            => Result.Merge(new[] { this }.Concat(results).ToArray());
    }
}
