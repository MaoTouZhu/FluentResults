using System;

namespace FluentResults.Samples.DomainDrivenDesign
{
    // 实体
    public class Customer
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public Address Address { get; private set; }

        private Customer()
        {
            Id = Guid.NewGuid();
        }

        // 此场景不需要返回 Result<Customer>，因为该方法不包含校验
        public static Customer Create()
        {
            return new Customer();
        }

        // 返回一个表示操作成功或失败的 Result 对象
        public Result AddAddress(Address address)
        {
            // 执行业务规则，不满足时返回失败结果
            if (address.City != "New York City")
                return Result.Fail("Address of customer should be in New York City.");

            Address = address;

            // 返回成功结果
            return Result.Ok();
        }
    }
}