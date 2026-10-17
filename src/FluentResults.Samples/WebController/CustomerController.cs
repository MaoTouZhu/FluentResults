namespace FluentResults.Samples.WebController
{
    // 这是一个 ASP.NET WebApi 控制器
    public class CustomerController
    {
        public ResultDto CreateCustomer(CreateCustomerDto model)
        {
            var result = CreateCustomerInternal(model);

            // 在系统边界使用自定义 ResultDto 类，让公共 API 由你负责。
            // 尽可能晚地把 FluentResult 的 Result 对象转换成自定义 ResultDto 对象。
            return result.ToResultDto();
        }

        private Result CreateCustomerInternal(CreateCustomerDto model)
        {
            // 创建客户的业务逻辑
            return Result.Ok();
        }
    }

    public class CreateCustomerDto
    {
        // 一些客户属性
    }
}