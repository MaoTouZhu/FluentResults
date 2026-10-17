using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// Success 类的对象不会导致失败的结果
    /// </summary>
    public class Success : ISuccess
    {
        /// <summary>
        /// 成功消息
        /// </summary>
        public string Message { get; protected set; }

        /// <summary>
        /// 成功的元数据
        /// </summary>
        public Dictionary<string, object> Metadata { get; }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        protected Success()
        {
            Metadata = new Dictionary<string, object>();
        }

        /// <summary>
        /// 创建 <see cref="Success"/> 的新实例并初始化 <see cref="Message"/> 属性
        /// </summary>
        /// <param name="message">消息</param>
        public Success(string message) : this()
        {
            Message = message;
        }

        /// <summary>
        /// 设置元数据
        /// </summary>
        public Success WithMetadata(string metadataName, object metadataValue)
        {
            Metadata.Add(metadataName, metadataValue);
            return this;
        }

        /// <summary>
        /// 设置元数据
        /// </summary>
        public Success WithMetadata(Dictionary<string, object> metadata)
        {
            foreach (var metadataItem in metadata)
            {
                Metadata.Add(metadataItem.Key, metadataItem.Value);
            }
            
            return this;
        }

        /// <summary>
        /// 重写的 ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return new ReasonStringBuilder()
                .WithReasonType(GetType())
                .WithInfo(nameof(Message), Message)
                .WithInfo(nameof(Metadata), string.Join("; ", Metadata))
                .Build();
        }
    }
}