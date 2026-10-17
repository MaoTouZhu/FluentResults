<h1>
  <img src="https://raw.githubusercontent.com/altmann/FluentResults/master/resources/icons/FluentResults-Icon-64.png" alt="FluentResults"/>
  FluentResults 
</h1>

[![Nuget downloads](https://img.shields.io/nuget/v/fluentresults.svg)](https://www.nuget.org/packages/FluentResults/)
[![Nuget](https://img.shields.io/nuget/dt/fluentresults)](https://www.nuget.org/packages/FluentResults/)
[![Build status](https://dev.azure.com/altmann/FluentResults/_apis/build/status/FluentResults-CI)](https://dev.azure.com/altmann/FluentResults/_build/latest?definitionId=11)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/altmann/FluentResults/blob/master/LICENSE)

**FluentResults 是一个轻量级的 .NET 库，用于解决一个常见问题。它返回一个表示操作成功或失败的对象，而不是抛出或使用异常。**

你可以通过 [NuGet 安装 FluentResults](https://www.nuget.org/packages/FluentResults/)：

```
Install-Package FluentResults
```

> :heart: 社区最需要的功能已发布到 NuGet：**[FluentResults.Extensions.AspNetCore](https://www.nuget.org/packages/FluentResults.Extensions.AspNetCore/)** 阅读[文档](https://github.com/altmann/FluentResults/wiki/Returning-Result-Objects-from-ASP.NET-Core-Controller)。试用它、测试它、[给予反馈](https://github.com/altmann/FluentResults/issues/149)。

## 主要特性

- **通用容器**，适用于所有场景（ASP.NET MVC/WebApi、WPF、DDD 领域模型等）
- 在一个 Result 对象中存储**多个错误**
- 存储**强大且细致的 Error 和 Success 对象**，而不只是字符串格式的错误消息
- 以面向对象的方式设计 Errors/Success
- 以分层方式存储**根因及错误链**
- 提供
  - .NET Standard、.NET Core、.NET 5+ 和 .NET Full Framework 支持（详情参见 [.NET 目标框架](https://github.com/altmann/FluentResults#net-targeting)）
  - SourceLink 支持
  - 强大的[代码示例](https://github.com/altmann/FluentResults#samplesbest-practices)，展示与著名或常见框架/库的集成
- **新增** 增强的 [FluentAssertions 扩展](https://github.com/altmann/FluentResults/wiki/Asserting-Result-Objects)，以优雅的方式对 FluentResult 对象进行断言
- **预览中** [从 ASP.NET Controller 返回 Result 对象](https://github.com/altmann/FluentResults/wiki/Returning-Result-Objects-from-ASP.NET-Core-Controller)

## 为什么用结果而不是异常

说实话，这种模式——返回一个表示成功或失败的 Result 对象——完全不是什么新想法。这种模式来自函数式编程语言。借助 FluentResults，这种模式也可以应用于 .NET/C#。 

文章[《Exceptions for Flow Control》，作者 Vladimir Khorikov](https://enterprisecraftsmanship.com/posts/exceptions-for-flow-control/)很好地说明了在哪些场景下 Result 模式有意义、哪些场景下没有意义。参见[最佳实践列表](https://github.com/altmann/FluentResults#samplesbest-practices)和[资源列表](https://github.com/altmann/FluentResults#interesting-resources-about-result-pattern)以进一步了解 Result 模式。

## 创建结果

一个 Result 可以存储多个 Error 和 Success 消息。

```csharp
// create a result which indicates success
Result successResult1 = Result.Ok();

// create a result which indicates failure
Result errorResult1 = Result.Fail("My error message");
Result errorResult2 = Result.Fail(new Error("My error message"));
Result errorResult3 = Result.Fail(new StartDateIsAfterEndDateError(startDate, endDate));
Result errorResult4 = Result.Fail(new List<string> { "Error 1", "Error 2" });
Result errorResult5 = Result.Fail(new List<IError> { new Error("Error 1"), new Error("Error 2") });
```
    
类 `Result` 通常用于没有返回值的 void 方法。

```csharp
public Result DoTask()
{
    if (this.State == TaskState.Done)
        return Result.Fail("Task is in the wrong state.");

    // rest of the logic

    return Result.Ok();
}
```

此外，如有必要，也可以存储特定类型的值。

```csharp
// create a result which indicates success
Result<int> successResult1 = Result.Ok(42);
Result<MyCustomObject> successResult2 = Result.Ok(new MyCustomObject());

// create a result which indicates failure
Result<int> errorResult = Result.Fail<int>("My error message");
```

类 `Result<T>` 通常用于有返回值的方法。

```csharp
public Result<Task> GetTask()
{
    if (this.State == TaskState.Deleted)
        return Result.Fail<Task>("Deleted Tasks can not be displayed.");

    // rest of the logic

    return Result.Ok(task);
}
```

## 处理结果

从方法获得 Result 对象后，你必须处理它。这意味着你需要检查操作是否成功完成。Result 对象中的属性 `IsSuccess` 和 `IsFailed` 表示成功或失败。`Result<T>` 的值可以通过属性 `Value` 和 `ValueOrDefault` 访问。

```csharp
Result<int> result = DoSomething();
     
// get all reasons why result object indicates success or failure. 
// contains Error and Success messages
IEnumerable<IReason> reasons = result.Reasons;

// get all Error messages
IEnumerable<IError> errors = result.Errors;

// get all Success messages
IEnumerable<ISuccess> successes = result.Successes;

if (result.IsFailed)
{
    // handle error case
    var value1 = result.Value; // throws exception because result is in failed state
    var value2 = result.ValueOrDefault; // return default value (=0) because result is in failed state
    return;
}

// handle success case
var value3 = result.Value; // return value and doesn't throw exception because result is in success state
var value4 = result.ValueOrDefault; // return value because result is in success state
```

## 设计错误和成功消息

有许多 Result 库只存储简单的字符串消息。FluentResults 则存储强大的面向对象 Error 和 Success 对象。其优势在于，错误或成功的所有相关信息都封装在一个类中。

该库的整个公共 API 都使用接口 `IReason`、`IError` 和 `ISuccess` 来表示原因、错误或成功。`IError` 和 `ISuccess` 继承自 `IReason`。如果 `Reasons` 属性中至少存在一个 `IError` 对象，那么结果表示失败，且属性 `IsSuccess` 为 false。

你可以通过继承 `ISuccess` 或 `IError`，或者继承 `Success` 或 `Error` 来创建自己的 `Success` 或 `Error` 类。

```csharp
public class StartDateIsAfterEndDateError : Error
{
    public StartDateIsAfterEndDateError(DateTime startDate, DateTime endDate)
        : base($"The start date {startDate} is after the end date {endDate}")
    { 
        Metadata.Add("ErrorCode", "12");
    }
}
```

借助这一机制，你也可以创建一个 `Warning` 类。你可以通过继承 `Success` 或 `Error` 类来选择系统中的 Warning 表示成功还是失败。  

## 更多功能

### 链式添加错误和成功消息

在某些情况下，需要在一个结果对象中链式添加多个错误和成功消息。

```csharp
var result = Result.Fail("error message 1")
                   .WithError("error message 2")
                   .WithError("error message 3")
                   .WithSuccess("success message 1");
```

### 根据成功/失败条件创建结果

你经常需要根据条件创建失败或成功的结果。通常你可以这样写：

```csharp
var result = string.IsNullOrEmpty(firstName) ? Result.Fail("First Name is empty") : Result.Ok();
```

使用 ```FailIf()``` 和 ```OkIf()``` 方法，你还可以写出更易读的代码。你也可以提供一个错误集合：

```csharp
var result = Result.FailIf(string.IsNullOrEmpty(firstName), "First Name is empty");

bool isValid = false; // Some check
var errors = new List<Error>{ new Error("Error 1"), new Error("Error 2") };

var result2 = Result.FailIf(isValid, errors);
```

如果你的成功检查基于是否存在错误，可以使用 `FailIfNotEmpty()`

```csharp
var errors = PerformSomeValidation();

var result = Result.FailIfNotEmpty(errors);
```

如果某个错误实例应当延迟初始化，为此可以使用接受 ```Func<string>``` 或 ```Func<IError>``` 的重载：

```csharp
var list = Enumerable.Range(1, 9).ToList();

var result = Result.FailIf(
    list.Any(IsDivisibleByTen),
    () => new Error($"Item {list.First(IsDivisibleByTen)} should not be on the list"));

bool IsDivisibleByTen(int i) => i % 10 == 0;

var errors = PerformSomeValidation();

var result = Result.FailIfNotEmpty(
    errors,
    (err) => new Error("Custom error message based on err"));

// rest of the code
```

### Try

在某些场景中，你想执行一个动作。如果该动作抛出异常，那么应当捕获该异常并将其转换为结果对象。

```csharp
var result = Result.Try(() => DoSomethingCritical());
```

你也可以返回自己的 `Result` 对象

```csharp
var result = Result.Try(() => {
    if(IsInvalid()) 
    {
        return Result.Fail("Some error");
    }

    int id = DoSomethingCritical();

    return Result.Ok(id);
});
```

在上面的示例中使用了默认的 catchHandler。默认 catchHandler 的行为可以通过全局 Result 设置覆盖（参见下一个示例）。你可以控制 Error 对象的样子。

```csharp
Result.Setup(cfg =>
{
    cfg.DefaultTryCatchHandler = exception =>
    {
        if (exception is SqlTypeException sqlException)
            return new ExceptionalError("Sql Fehler", sqlException);

        if (exception is DomainException domainException)
            return new Error("Domain Fehler")
                .CausedBy(new ExceptionError(domainException.Message, domainException));

        return new Error(exception.Message);
    };
});

var result = Result.Try(() => DoSomethingCritical());
```

也可以通过 ```Try(..)``` 方法传入自定义 catchHandler。

```csharp
var result = Result.Try(() => DoSomethingCritical(), ex => new MyCustomExceptionError(ex));
```

### 错误的根因

你也可以在错误对象中存储错误的根因。通过方法 `CausedBy(...)`，根因可以作为 Error、Errors 列表、字符串、字符串列表或异常传入。根因存储在错误对象的 `Reasons` 属性中。

示例 1 - 根因是一个异常
```csharp
try
{
    //export csv file
}
catch(CsvExportException ex)
{
    return Result.Fail(new Error("CSV Export not executed successfully").CausedBy(ex));
}
```

示例 2 - 根因是一个错误
```csharp
Error rootCauseError = new Error("This is the root cause of the error");
Result result = Result.Fail(new Error("Do something failed", rootCauseError));
```

示例 3 - 从错误中读取根因
```csharp
Result result = ....;
if (result.IsSuccess)
   return;

foreach(IError error in result.Errors)
{
    foreach(ExceptionalError causedByExceptionalError in error.Reasons.OfType<ExceptionalError>())
    {
        Console.WriteLine(causedByExceptionalError.Exception);
    }
}
```

### 元数据

你可以向 Error 或 Success 对象添加元数据。

一种方式是在创建结果对象时直接调用方法 `WithMetadata(...)`。

```csharp
var result1 = Result.Fail(new Error("Error 1").WithMetadata("metadata name", "metadata value"));

var result2 = Result.Ok()
                    .WithSuccess(new Success("Success 1")
                                 .WithMetadata("metadata name", "metadata value"));
```

另一种方式是在 `Error` 或 `Success` 类的构造函数中调用 `WithMetadata(...)`。

```csharp
public class DomainError : Error
{
    public DomainError(string message)
        : base(message)
    { 
        WithMetadata("ErrorCode", "12");
    }
}
```

### 合并

多个结果可以使用静态方法 `Merge()` 合并。

```csharp
var result1 = Result.Ok();
var result2 = Result.Fail("first error");
var result3 = Result.Ok<int>();

var mergedResult = Result.Merge(result1, result2, result3);
```

结果列表可以通过扩展方法 `Merge()` 合并为一个结果。

```csharp
var result1 = Result.Ok();
var result2 = Result.Fail("first error");
var result3 = Result.Ok<int>();

var results = new List<Result> { result1, result2, result3 };

var mergedResult = results.Merge();
```

你也可以使用 `MergeFlat()` 将包含元素集合的结果合并为一个展平的集合。值类型和 Enumerable 类型必须作为泛型参数指定

```csharp
var result1 = Result.Ok(new string[] { "A", "B" });
var result2 = Result.Ok(new string[] { "C", "D" });
var result3 = Result.Ok(new string[] { "E", "F" });

// Will contain ["A", "B", "C", "D", "E", "F"]
var mergedResult = Result.MergeFlat<string, string[]>(result1, result2, result3);
```

### 转换与变换

结果对象可以通过方法 `ToResult()` 和 `ToResult<TValue>()` 转换为另一个结果对象。

```csharp
// converting a result to a result from type Result<int> with default value of int
Result.Ok().ToResult<int>();

// converting a result to a result from type Result<int> with a custom value
Result.Ok().ToResult<int>(5);

// converting a failed result to a result from type Result<int> without passing a custom value
// because result is in failed state and therefore no value is needed
Result.Fail("Failed").ToResult<int>();

// converting a result to a result from type Result<float>
Result.Ok<int>(5).ToResult<float>(v => v);

// converting a result from type Result<int> to result from type Result<float> without passing the converting
// logic because result is in failed state and therefore no converting logic needed
Result.Fail<int>("Failed").ToResult<float>();

// converting a result to a result from type Result
Result.Ok<int>().ToResult();
```

结果对象的值可以通过方法 ``Map(..)` 变换为另一个值

```csharp
// converting a result to a result from type Result<float>
Result.Ok<int>(5).Map(v => new Dto(5));
```

### 从 T 隐式转换为成功结果 ```Result<T>```

```csharp
string myString = "hello world";
Result<T> result = myString;
```

### 从 Error 隐式转换为失败结果 ```Result``` 或 ```Result<T>```

从单个错误

```csharp
Error myError = new Error("error msg");
Result result = myError;
```
或者从错误列表

```csharp
List<Error> myErrors = new List<Error>() 
    { 
        new Error("error 1"), 
        new Error("error 2") 
    };
    
Result result = myErrors;
```

### 将结果绑定到另一个结果

绑定是一种返回 `Result` | `Result<T>` 的转换。
它仅在原始结果成功时才执行转换。
两个 `Result` 的原因会合并到一个新的展平 `Result` 中。

```csharp
// converting a result to a result which may fail
Result<string> r = Result.Ok(8)
    .Bind(v => v == 5 ? "five" : Result.Fail<string>("It is not five"));

// converting a failed result to a result, which can also fail, 
// returns a result with the errors of the first result only,
// the transformation is not evaluated because the value of the first result is not available
Result<string> r = Result.Fail<int>("Not available")
    .Bind(v => v == 5 ? "five" : Result.Fail<string>("It is not five"));

// converting a result with value to a Result via a transformation which may fail
Result.Ok(5).Bind(x => Result.OkIf(x == 6, "Number is not 6"));

// converting a result without value into a Result 
Result.Ok().Bind(() => Result.Ok(5));

// just running an action if the original result is sucessful. 
Result r = Result.Ok().Bind(() => Result.Ok());
```

`Bind` 有异步重载。

```csharp
var result = await Result.Ok(5)
    .Bind(int n => Task.FromResult(Result.Ok(n + 1).WithSuccess("Added one")))
    .Bind(int n => /* next continuation */);
```

### 为 ISuccess/IError/IExceptionalError 设置全局工厂

在 FluentResults 库内部，某些场景下会创建 ISuccess、IError 或 IExceptionalError 对象。例如，如果调用方法 ```Result.Fail("My Error")```，那么内部会创建一个 IError 对象。如果你需要覆盖此行为，并在该场景中创建自定义错误类，那么可以在设置中指定错误工厂。同样的扩展点也适用于 ISuccess 和 IExceptionalError。

```csharp
Result.Setup(cfg =>
{
    cfg.SuccessFactory = successMessage => new Success(successMessage).WithMetadata("Timestamp", DateTime.Now);
    
    cfg.ErrorFactory = errorMessage => new Error(errorMessage).WithMetadata("Timestamp", DateTime.Now);
    
    cfg.ExceptionalErrorFactory = (errorMessage, exception) => new ExceptionalError(errorMessage ?? exception.Message, exception)
    .WithMetadata("Timestamp", DateTime.Now);
});
```

### 映射错误和成功

如果你想为结果中的所有成功添加一些信息，可以在结果对象上使用 `MapSuccesses(...)`。

```csharp
var result = Result.Ok().WithSuccess("Success 1");
var result2 = result.MapSuccesses(e => new Success("Prefix: " + e.Message));
```

如果你想为结果中的所有错误添加一些信息，可以在结果对象上使用 `MapErrors(...)`。此方法只遍历第一层的错误，根因错误（位于 error.Reasons 中）不会被更改。

```csharp
var result = Result.Fail("Error 1");
var result2 = result.MapErrors(e => new Error("Prefix: " + e.Message));
```

### 处理/捕获错误

与异常的 catch 块类似，Result 对象内部的错误检查与处理也通过一些方法得到支持：

```csharp
// check if the Result object contains an error from a specific type
result.HasError<MyCustomError>();

// check if the Result object contains an error from a specific type and with a specific condition
result.HasError<MyCustomError>(myCustomError => myCustomError.MyField == 2);

// check if the Result object contains an error with a specific metadata key
result.HasError(error => error.HasMetadataKey("MyKey"));

// check if the Result object contains an error with a specific metadata
result.HasError(error => error.HasMetadata("MyKey", metadataValue => (string)metadataValue == "MyValue")); 
```

所有 ```HasError()``` 方法都有一个可选的 out 参数 result，用于访问找到的错误。

### 处理成功

检查结果对象是否包含特定的成功对象可以通过方法 `HasSuccess()` 完成

```csharp
// check if the Result object contains a success from a specific type
result.HasSuccess<MyCustomSuccess>();

// check if the Result object contains a success from a specific type and with a specific condition
result.HasSuccess<MyCustomSuccess>(success => success.MyField == 3);
```

所有 ```HasSuccess()``` 方法都有一个可选的 out 参数 result，用于访问找到的成功项。

### 处理/捕获异常

检查结果对象是否包含具有特定异常类型的错误可以通过方法 `HasException()` 完成

```csharp
// check if the Result object contains an exception from a specific type
result.HasException<MyCustomException>();

// check if the Result object contains an exception from a specific type and with a specific condition
result.HasException<MyCustomException>(MyCustomException => MyCustomException.MyField == 1);
```

所有 ```HasException()``` 方法都有一个可选的 out 参数 result，用于访问找到的错误。

### 模式匹配

```csharp
var result = Result.Fail<int>("Error 1");

var outcome = result switch
{
     { IsFailed: true } => $"Errored because {result.Errors}",
     { IsSuccess: true } => $"Value is {result.Value}",
     _ => null
};
```

### Golang 风格的解构运算符

```csharp
// For Result<TValue> you get TValue and Errors
var (result, errors) = Result.Ok(1);
var (result, errors) = Result.Ok<int>(1);
// beware that on error you will get "result==0" (since TValue is a non-nullable int), so you probably want to check errors first!
var (result, errors) = Result.Fail<int>("fail");

// For Result (without underlying value) you get bool isSuccess and Errors
var (isSuccess, errors) = Result.Fail("Failure 1");
var (isSuccess, errors) = Result.Ok();
```

### 日志

有时需要记录结果的日志。首先创建一个日志记录器：

```csharp
public class MyConsoleLogger : IResultLogger
{
    public void Log(string context, string content, ResultBase result, LogLevel logLevel)
    {
        Console.WriteLine("Result: {0} {1} <{2}>", result.Reasons.Select(reason => reason.Message), content, context);
    }

    public void Log<TContext>(string content, ResultBase result, LogLevel logLevel)
    {
        Console.WriteLine("Result: {0} {1} <{2}>", result.Reasons.Select(reason => reason.Message), content, typeof(TContext).FullName);
    }
}
```

然后你必须在 Result 设置中注册你的日志记录器：

```csharp
var myLogger = new MyConsoleLogger();
Result.Setup(cfg => {
    cfg.Logger = myLogger;
});
```

最后，日志记录器可用于任何结果：

```csharp
var result = Result.Fail("Operation failed")
    .Log();
```

此外，还可以以字符串或泛型类型参数的形式传入上下文。也可以将提供更多信息的自定义消息作为内容传入。

```csharp
var result = Result.Fail("Operation failed")
    .Log("logger context", "More info about the result");

var result2 = Result.Fail("Operation failed")
    .Log<MyLoggerContext>("More info about the result");
```

也可以指定所需的日志级别：
```csharp
var result = Result.Ok().Log(LogLevel.Debug);
var result = Result.Fail().Log<MyContext>("Additional context", LogLevel.Error);
```

你也可以仅在成功或失败时记录结果日志：

```csharp
Result<int> result = DoSomething();

// log with default log level 'Information'
result.LogIfSuccess();

// log with default log level 'Error'
result.LogIfFailed();
```


### 断言 FluentResult 对象

试试 FluentAssertions 与 [FluentResults.Extensions.FluentAssertions](https://github.com/altmann/FluentResults/wiki#fluentresultsextensionsfluentassertions) 的强大能力。自 v2.0 起，断言包已走出实验阶段，它确实是以流式方式断言结果对象的绝佳增强。

### .NET 目标框架

FluentResults 3.x 及以上版本支持 .NET Standard 2.0 和 .NET Standard 2.1。
如果你需要支持 .NET Standard 1.1、.NET 4.6.1 或 .NET 4.5，请使用 [FluentResults 2.x](https://www.nuget.org/packages/FluentResults/2.6.0)。

## 示例/最佳实践

以下是一些示例和最佳实践，供在使用 FluentResult 或通常的 Result 模式搭配著名或常用框架和库时参考遵循。

### 受领域驱动设计启发的强大领域模型

- [带命令处理器的领域模型](https://github.com/altmann/FluentResults/tree/master/src/FluentResults.Samples/DomainDrivenDesign)
- 通过使用例如返回 Result 对象的工厂方法来保护领域不变量
- 通过创建继承自 IError 接口或 Error 类的自定义 Error 类，使每个错误唯一
- 如果方法没有失败场景，则不要使用 Result 类作为返回类型
- 请注意，你可以合并多个失败结果，或尽快返回第一个失败结果

### 序列化 Result 对象（ASP.NET WebApi、[Hangfire](https://www.hangfire.io/)）

- [Asp.net WebController](https://github.com/altmann/FluentResults/tree/master/src/FluentResults.Samples/WebController)
- [Hangfire 作业](https://github.com/altmann/FluentResults/tree/master/src/FluentResults.Samples/HangfireJobs)
- 不要序列化 FluentResult 结果对象。
- 在系统边界内为你的公共 API 创建你自己的自定义 ResultDto 类
  - 这样你可以控制哪些数据被提交、哪些数据被序列化
  - 你的公共 API 独立于 FluentResults 等第三方库
  - 你可以保持公共 API 稳定

### [MediatR](https://github.com/jbogard/MediatR) 请求处理器返回 Result 对象

- [包含命令/查询和 ValidationPipelineBehavior 的完整功能性 .NET Core 示例代码](https://github.com/altmann/FluentResults/tree/master/src/FluentResults.Samples.MediatR)
- 通过 Result 对象将业务验证错误从 MediatR 请求处理器返回给调用方
- 不要基于业务验证错误抛出异常
- 通过 MediatR PipelineBehavior 注入命令和查询验证，并返回 Result 对象而不是抛出异常

## 关于 Result 模式的有趣资源

- [错误处理 — 返回结果，作者 Michael Altmann](https://medium.com/@michael_altmann/error-handling-returning-results-2b88b5ea11e9)
- [操作结果模式，作者 Carl-Hugo Marcotte](https://www.forevolve.com/en/articles/2018/03/19/operation-result/)
- [C# 中用于流程控制的异常，作者 Vladimir Khorikov](https://enterprisecraftsmanship.com/posts/exceptions-for-flow-control/)
- [错误处理：异常还是结果？作者 Vladimir Khorikov](https://enterprisecraftsmanship.com/posts/error-handling-exception-or-result/)
- [代码中什么是异常情况？作者 Vladimir Khorikov](https://enterprisecraftsmanship.com/posts/what-is-exceptional-situation/)
- [高级错误处理技术，作者 Vladimir Khorikov](https://enterprisecraftsmanship.com/posts/advanced-error-handling-techniques/)
- [简明指南，作者 Isaac Cummings](https://medium.com/@cummingsi1993/the-operation-result-pattern-a-simple-guide-fe10ff959080)
- [使用 Result 类的灵活错误处理，作者 Khalil Stemmler](https://khalilstemmler.com/articles/enterprise-typescript-nodejs/handling-errors-result-class/)
- [将 ASP.NET Core 验证特性与值对象结合，作者 Vladimir Khorikov](https://enterprisecraftsmanship.com/posts/combining-asp-net-core-attributes-with-value-objects/)

## 捐赠

我喜欢这个项目，但实现功能、回复 issue 或维护 CI/发布流水线都需要时间——这是我的业余时间。如果你喜欢 FluentResult 并且觉得它有用，可以考虑捐赠。点击右上角的赞助按钮。

## 贡献者

感谢所有贡献者以及所有提供反馈的人！

<a href="https://github.com/altmann/fluentresults/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=altmann/fluentresults" />
</a>

## 版权

版权所有 (c) Michael Altmann。详情参见 [LICENSE](https://raw.githubusercontent.com/altmann/FluentResults/master/LICENSE)。
