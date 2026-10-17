using System;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// <see cref="IReason"/> 的扩展方法
    /// </summary>
    public static class ReasonExtensions
    {
        /// <summary>
        /// 检查元数据键是否存在
        /// </summary>
        /// <param name="reason">原因实例</param>
        /// <param name="key">元数据键</param>
        /// <returns>如果元数据键存在，则为 true</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool HasMetadataKey(this IReason reason, string key)
        {
            if(string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            return reason.Metadata.ContainsKey(key);
        }

        /// <summary>
        /// 检查元数据键是否存在且与提供的谓词匹配
        /// </summary>
        /// <param name="reason">原因实例</param>
        /// <param name="key">元数据键</param>
        /// <param name="predicate">用于检查元数据键是否存在的谓词</param>
        /// <returns>如果元数据值与谓词匹配，则为 true</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool HasMetadata(this IReason reason, string key, Func<object, bool> predicate)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            if (reason.Metadata.TryGetValue(key, out object actualValue))
                return predicate(actualValue);

            return false;
        }
    }
}