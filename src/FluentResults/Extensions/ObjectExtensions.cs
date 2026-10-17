// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// object 基类型的扩展方法
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// 将值转换为结果
        /// </summary>
        /// <typeparam name="TValue">结果的 <see cref="Result{TValue}.Value"/> 的类型</typeparam>
        /// <param name="value">结果的值</param>
        public static Result<TValue> ToResult<TValue>(this TValue value)
        {
            return new Result<TValue>()
                .WithValue(value);
        }

        internal static string ToLabelValueStringOrEmpty(this object value, string label)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var valueText = value.ToString();

            if (valueText == string.Empty)
            {
                return string.Empty;
            }

            return $"{label}='{valueText}'";
        }
    }
}