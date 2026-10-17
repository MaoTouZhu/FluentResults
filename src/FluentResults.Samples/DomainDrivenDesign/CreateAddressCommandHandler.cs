namespace FluentResults.Samples.DomainDrivenDesign
{
    public class CreateAddressCommandHandler
    {
        private readonly CustomerRepository _repository;

        public CreateAddressCommandHandler(CustomerRepository repository)
        {
            _repository = repository;
        }

        public Result Handle(CreateAddressCommand command)
        {
            // 如果地址数据无效，createAddressResult 会包含所有错误消息
            var createAddressResult = Address.Create(command.Street, command.Number, command.City);

            // 地址无效时返回失败结果，并尽快返回
            if (createAddressResult.IsFailed)
                return createAddressResult.ToResult();

            var customer = _repository.GetById(command.CustomerId);

            var addAddressResult = customer.AddAddress(createAddressResult.Value);

            // 因业务规则无法添加地址时返回失败结果，并尽快返回
            if (addAddressResult.IsFailed)
                return addAddressResult;

            // 所有操作提交完成，若没有失败则返回成功结果
            _repository.Commit();
            return Result.Ok();
        }
    }
}
