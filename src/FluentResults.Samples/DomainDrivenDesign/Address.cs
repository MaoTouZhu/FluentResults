namespace FluentResults.Samples.DomainDrivenDesign
{
    // 值对象
    public class Address
    {
        public string Street { get; private set; }

        public string Number { get; private set; }

        public string City { get; private set; }

        private Address()
        {
        }

        public static Result<Address> Create(string street, string number, string city)
        {
            var validationResult = Result.Merge(
                ValidateStreet(street), 
                ValidateNumber(number));

            // 传入参数无效时返回失败结果
            if (validationResult.IsFailed)
                return validationResult;

            return Result.Ok(new Address
            {
                Street = street,
                Number = number,
                City = city
            });
        }

        private static Result ValidateNumber(string number)
        {
            // 方式 1：用字符串消息设置错误
            return Result.FailIf(string.IsNullOrEmpty(number), "Number is required.");

            // 方式 2：用 Error 对象设置错误
            // return Result.FailIf(string.IsNullOrEmpty(number), new Error("Number is required."));

            // 方式 3：用自定义 Error 对象设置错误
            // return Result.FailIf(string.IsNullOrEmpty(number), new RequiredError("Number"));
        }

        private static Result ValidateStreet(string street)
        {
            // 多个结果合并为一个结果
            return Result.Merge(
                Result.FailIf(string.IsNullOrEmpty(street), "Street is required."),
                Result.FailIf(!string.IsNullOrEmpty(street) && street.Length > 30,
                    "Street should contain max 30 characters.")
            );
        }
    }

    public class RequiredError : Error
    {
        public string FieldName { get; private set; }
        
        public RequiredError(string fieldName) 
            : base($"{fieldName} is required.")
        {
            FieldName = fieldName;
            Metadata.Add("ErrorCode", 100);
        }
    }
}