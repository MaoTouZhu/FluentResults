using System;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// <see cref="ResultSettings"/> 的构建器
    /// </summary>
    public class ResultSettingsBuilder
    {
        /// <summary>
        /// 设置 ResultLogger
        /// </summary>
        public IResultLogger Logger { get; set; }

        /// <summary>
        /// 用于创建 IError 对象的工厂。在 Try 方法中创建错误的所有场景中使用
        /// </summary>
        public Func<Exception, IError> DefaultTryCatchHandler { get; set; }

        /// <summary>
        /// 用于创建 ISuccess 对象的工厂。在 FluentResults 中创建成功的所有场景中使用。 
        /// </summary>
        public Func<string, ISuccess> SuccessFactory { get; set; }

        /// <summary>
        /// 用于创建 IError 对象的工厂。在 FluentResults 中创建错误的所有场景中使用。 
        /// </summary>
        public Func<string, IError> ErrorFactory { get; set; }
        
        /// <summary>
        /// 用于创建 IExceptionalError 对象的工厂。在 FluentResults 中创建异常错误的所有场景中使用。 
        /// </summary>
        public Func<string, Exception, IExceptionalError> ExceptionalErrorFactory { get; set; }

        /// <summary>
        /// 设置默认值的默认构造函数
        /// </summary>
        public ResultSettingsBuilder()
        {
            // 设置默认值
            Logger = new DefaultLogger();
            DefaultTryCatchHandler = ex => Result.Settings.ExceptionalErrorFactory(ex.Message, ex);
            SuccessFactory = successMessage => new Success(successMessage);
            ErrorFactory = errorMessage => new Error(errorMessage);
            ExceptionalErrorFactory = (errorMessage, exception) => new ExceptionalError(errorMessage ?? exception.Message, exception);
        }

        /// <summary>
        /// 使用当前值创建 <see cref="ResultSettings"/> 对象
        /// </summary>
        /// <returns></returns>
        public ResultSettings Build()
        {
            return new ResultSettings
            {
                Logger = Logger,
                DefaultTryCatchHandler = DefaultTryCatchHandler,
                SuccessFactory = SuccessFactory,
                ErrorFactory = ErrorFactory,
                ExceptionalErrorFactory = ExceptionalErrorFactory
            };
        }
    }
}