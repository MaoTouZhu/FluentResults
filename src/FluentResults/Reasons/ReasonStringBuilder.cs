using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// 创建序列化原因实例时显示的文本
    /// </summary>
    public class ReasonStringBuilder
    {
        private string _reasonType = string.Empty;
        private readonly List<string> _infos = new List<string>();
         
        /// <summary>
        /// 指定原因的类型
        /// </summary>
        /// <param name="type">原因的类型</param>
        /// <returns>对当前 Builder 的引用</returns>
        public ReasonStringBuilder WithReasonType(Type type)
        {
            _reasonType = type.Name;
            return this;
        }

        /// <summary>
        /// 为给定值添加信息标签
        /// </summary>
        /// <param name="label">要显示的标签</param>
        /// <param name="value">标签所对应的值</param>
        /// <returns>对当前 Builder 的引用</returns>
        public ReasonStringBuilder WithInfo(string label, string value)
        {
            var infoString = value.ToLabelValueStringOrEmpty(label);

            if(!string.IsNullOrEmpty(infoString))
            {
                _infos.Add(infoString);
            }

            return this;
        }

        /// <summary>
        /// 根据当前信息创建原因字符串
        /// </summary>
        /// <returns>原因字符串</returns>
        public string Build()
        {
            var reasonInfoText = _infos.Any()
                ? " with " + ReasonInfosToString(_infos)
                : string.Empty;

            return $"{_reasonType}{reasonInfoText}";
        }

        private static string ReasonInfosToString(List<string> reasonInfos)
        {
            return string.Join(", ", reasonInfos);
        }
    }
}