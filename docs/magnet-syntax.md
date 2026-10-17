# MagNet 风格语法契约（批次 1）

> 状态：**批次 1 已实现**（net8.0 全量测试通过；`netstandard2.0` 编译通过）。
> 本文档是实施契约，不是已发布 API。
> 版本：`rev 1.1`。
>
> 范围：`D-01 ~ D-05` + `MAG-001/002/003/004/006/007/008a/010/013a/014`。
>
> 不包含：`MAG-005/009/011/013b/015/016/019/022/023/025`（明确不做），
> `MAG-008b/012/017/018/020/021/024`（观察区，见第 8 节）。

---

## 1. 一页速查

| 写法 | 语义 | ID |
| --- | --- | --- |
| `Result r = "boom";` | 失败，`Error("boom")` | MAG-001 |
| `Result r = ex;` | 失败，`ExceptionalError(ex)` | MAG-002 |
| `Result r = true;` / `false` | 成功 / 失败 | MAG-003 |
| `if (r)` / `bool b = r;` | `IsSuccess` | MAG-004 |
| `throw r;` / `Exception e = r;` | 取异常并抛出 | MAG-006 |
| `r.ErrorMessage` | 所有错误消息用 `"; "` 拼接 | MAG-008a |
| `r.ThrowIfFailed();` / `r.Throw();` | 失败时抛异常 | MAG-010 |
| `"boom".ToError();` / `ex.ToError();` | 失败结果 | MAG-013a |
| `Result.DefaultErrorMessage` / `Result.DefaultError` | 默认文案 / 默认异常 | MAG-014 |
| `Result<int> r = "boom";` | 失败，值 `default` | D-01 |
| `Result<int> r = ex;` | 失败，值 `default` | D-02 |
| `int v = r;` | `ValueOrDefault` | D-03 |
| `if (r)` / `bool ok = r;` | `IsSuccess` | D-04 |
| `throw r;` / `Exception e = r;` | 取异常并抛出 | D-05 |
| `Result plain = r;` | 丢弃 `Value`，保留 `Reasons` | MAG-007 |

---

## 2. 非泛型 `Result`

### 2.1 字符串 = 错误信息

```csharp
Result r1 = "boom";          // 失败，Errors[0].Message == "boom"
Result r2 = (string)null;    // 失败，ErrorMessage == Result.DefaultErrorMessage
```

- 等价于 `Result.Fail(errorMessage)`，因此 `Result.Setup(...)` 配置的错误工厂仍然生效。
- `null` 不抛异常，按默认文案处理。

### 2.2 异常 = 失败（保留原实例）

```csharp
var ex = new InvalidOperationException("boom");

Result r = ex;               // 失败
bool isExceptional = r.Errors[0] is ExceptionalError;                  // true
bool sameInstance = ((IExceptionalError)r.Errors[0]).Exception == ex;  // true
```

- `null` → `Result.Fail(Result.DefaultErrorMessage)`。
- 与 `Result.Fail(new ExceptionalError(ex))` 等价，不会丢栈。

### 2.3 `bool` → `Result`（仅非泛型）

```csharp
Result ok   = true;    // 成功
Result fail = false;   // 失败，ErrorMessage == Result.DefaultErrorMessage

bool b = fail;         // false
if (ok) { }            // 可用
```

**不提供 `bool → Result<T>`**。`Result<bool> rb = true` 走的是已有 `T → Result<T>`（成功），不是这里。

### 2.4 真值、取异常与 `throw`

```csharp
Result failed = Result.Fail("boom");

if (failed) { }              // false
bool ok = Result.Ok();       // true

throw failed;                // 抛出 Exception("boom")
Exception e = failed;        // 普通错误每次按消息新建 Exception
```

> 若来源是 `ExceptionalError`，取到的是原异常实例；普通 `Error` 没有异常，
> 每次访问都会按 `ErrorMessage` 新建一个 `Exception`。

取异常的优先级：

1. 首个 `IExceptionalError.Exception`（原异常实例）；
2. 否则若失败，`new Exception(ErrorMessage)`；
3. 若成功，`Result.DefaultError`。

> `throw Result.Ok();` 会抛 `DefaultError`。`throw` 只应对失败结果使用。

### 2.5 `ErrorMessage`

```csharp
Result.Ok().ErrorMessage;                        // null
Result.Fail("a").ErrorMessage;                   // "a"
Result.Fail(new[] { "a", "b" }).ErrorMessage;    // "a; b"
```

- 只读视图，不改 `Errors` / `Reasons`。
- 多错误按 `"; "` 拼接；需要结构化访问时用 `Errors`。

### 2.6 `Throw` / `ThrowIfFailed`

```csharp
Result.Ok().ThrowIfFailed();          // 不抛
Result.Fail("boom").ThrowIfFailed();  // 抛 Exception("boom")
Result.Fail("boom").Throw();          // 抛
Result.Ok().Throw();                  // 抛 DefaultError
```

- 失败时按 2.4 的优先级取异常。
- `ThrowIfFailed` 是幂等的“守卫”；`Throw` 总是抛。

### 2.7 默认文案与默认异常

```csharp
Result.DefaultErrorMessage;   // "Undefined error"
Result.DefaultError;          // 进程级 Exception 单例
```

用于 2.1 / 2.2 / 2.3 的 `null` / `false` 兜底，避免每次新建异常。

### 2.8 `ToError` 扩展

```csharp
var ex = new InvalidOperationException("boom");

Result a = "boom".ToError();          // Result.Fail("boom")
Result<int> b = "boom".ToError<int>();// Result.Fail<int>("boom")
Result c = ex.ToError();              // Result.Fail(new ExceptionalError(ex))
Result<int> d = ex.ToError<int>();    // Result.Fail<int>(new ExceptionalError(ex))
```

与隐式转换的区别：`ToError` 是显式方法调用，可用于方法链和泛型推断；
`ToSuccess` 不新增，直接使用已有的 `ToResult()`。

---

## 3. 泛型 `Result<T>`

### 3.1 字符串 = 失败

```csharp
Result<int> r1 = "boom";          // 失败，ValueOrDefault == 0
Result<string> r2 = "boom";       // ❌ CS0457，见第 5 节
```

### 3.2 异常 = 失败

```csharp
Result<int> r1 = new InvalidOperationException("boom");  // 失败，ValueOrDefault == 0
Result<Exception> r2 = new InvalidOperationException();  // ❌ CS0457，见第 5 节
```

### 3.3 `Result<T>` → `T`

```csharp
Result<int> r = Result.Ok(42);

int a = r;                          // 42
int b = Result.Fail<int>("boom");   // 0（ValueOrDefault，默认值）
int c = Result.Fail<int>("boom").Value; // 抛 InvalidOperationException
```

- 隐式转换使用 **`ValueOrDefault`**：失败时静默给 `default`。
- 需要失败即抛时，显式访问 `.Value`。

### 3.4 `Result<T>` → `bool`

```csharp
Result<int> r = Result.Ok(42);

if (r) { }              // IsSuccess == true
bool ok = r;           // true
bool bad = Result.Fail<int>("x"); // false
```

- 单向：`bool → Result<T>` 不存在。
- `Result<bool>` 例外，见第 5 节。

### 3.5 `Result<T>` → `Exception`

```csharp
Result<int> r = Result.Fail<int>(new ExceptionalError(new InvalidOperationException("boom")));

throw r;                    // 抛原 InvalidOperationException
Exception e = r;            // 同一实例
```

优先级与 2.4 完全一致。

### 3.6 `Result<T>` → `Result`

```csharp
Result<int> withValue = Result.Ok(42);
Result plain = withValue;      // 成功，Value 丢弃
Result<int> back = plain;      // 成功，ValueOrDefault == 0（已有 Result → Result<T>）
```

- 实现为已有的 `ToResult()`。
- `Reasons` 中的 `IError` / `ISuccess` 对象按引用保留；**原 `Result<T>` 实例本身不保留**。
  若后续必须回溯原实例，需要新加 `Source` 一类属性（另立项，不在本批次）。

### 3.7 继承的成员

`Result<T>` 同样拥有 `ErrorMessage`、`Throw()`、`ThrowIfFailed()`，语义与非泛型一致。

---

## 4. 保留类型（重要）

`string`、`Exception`、`bool` 作为 `T` 时，**构造/取值方向上的隐式转换是有意二义的**：
这些类型被保留给语法糖，代价是 `Result<string>` / `Result<Exception>` / `Result<bool>`
不参与对应的隐式转换。

| ID | 关系 |
| --- | --- |
| D-01 | `string → Result<T>`（失败） |
| D-02 | `Exception → Result<T>`（失败） |
| D-03 | `Result<T> → T`（`ValueOrDefault`） |
| D-04 | `Result<T> → bool`（`IsSuccess`，单向） |
| D-05 | `Result<T> → Exception`（`throw` 糖） |

| 写法 | `Result<string>` | `Result<Exception>` | `Result<bool>` |
| --- | --- | --- | --- |
| `Result<T> r = value;` | ❌ CS0457 | ❌ CS0457 | ✅（`T→Result<T>`） |
| `T v = r;` | ✅ | ❌ CS0457（D-03 vs D-05） | ❌ CS0457（D-03 vs D-04） |
| `bool b = r;` | ✅ | ✅ | ❌ CS0457 |
| `if (r)` | ✅ | ✅ | ❌ CS0457 |
| `Exception e = r;` | ✅ | ❌ CS0457 | ✅ |
| `throw r;` | ✅ | ❌ CS0457 | ✅ |

### 正确写法

```csharp
// Result<string>
Result<string> a = Result.Ok("x");       // 不用隐式构造
string s = a;                            // 取值可以

// Result<Exception>
Result<Exception> b = Result.Ok<Exception>(ex);
Exception e = b.ValueOrDefault;          // 不用隐式取值

// Result<bool>
Result<bool> c = true;                   // 构造可以（T→Result<T>）
bool ok = c.IsSuccess;                   // 不用隐式取值 / if
bool val = c.ValueOrDefault;
```

---

## 5. 编译契约

```csharp
// ✅ 非泛型
Result a = "boom";
Result b = new Exception("x");
Result c = true;
if (a) { }
throw a;

// ✅ 泛型（非保留类型）
Result<int> d = "boom";
Result<int> e = new Exception("x");
int f = d;
if (d) { }
Result plain = d;

// ❌ 保留类型
Result<string> g = "boom";                  // CS0457
Result<Exception> h = new Exception("x");   // CS0457
Result<bool> i = true;                      // ✅ 构造成功
bool j = i;                                 // CS0457
if (i) { }                                  // CS0457
Exception k = h;                            // CS0457
```

---

## 6. 已知限制与副作用

| 现象 | 原因 | 规避 |
| --- | --- | --- |
| `Console.WriteLine(result)` 输出 `True` / `False` | 重载绑定到 `WriteLine(bool)` | 显式 `result.ToString()` |
| `Console.WriteLine(resultInt)` 会 CS0121 | `Result<int>→int` 与 `→bool` 两个重载都适用（实测） | 先取 `ValueOrDefault` 或 `ToString()` |
| `cond ? resultInt : resultPlain` CS0172 | `Result<T>` 与 `Result` 双向隐式转换 | 显式转换其中一个分支，或改 `if/else` |
| `int v = failedResult` 得到 `0`，不抛 | D-03 使用 `ValueOrDefault` | 需要失败即抛时用 `.Value` |
| `throw Result.Ok()` 抛 `DefaultError` | `throw` 无法区分成功/失败 | 只 `throw` 失败结果 |
| `Result<string>` / `Result<Exception>` / `Result<bool>` 不能用隐式构造/取值 | 保留类型的二义性 | 见第 4 节 |

---

## 7. 本批次不实现的 MagNet 语法

| MagNet 写法 | 本分支替代 |
| --- | --- |
| `Result.SetSuccess(...)` / `SetError(...)` | `Result.Ok(...)` / `Result.Fail(...)` |
| `result.Invoke(...)` / `InvokeAsync(...)` | `Result.Try(...)` |
| `result.Out(out var v)` | `Deconstruct` / `ValueOrDefault` |
| `result.Dump(tag)` | `ToString()` / 调试器 |
| `result.Clone()` | 重新构造 / Fluent 自有机制 |
| `result.Previous` / `Trace` / `Combine(...)` | `Reasons` / `Merge(...)` |
| `result.IsError` | `IsFailed` |
| `string s = result;` | `result.ToString()` |
| `● / ○` 版 `ToString` | 保留 Fluent 现有格式 |
| `bool → Result<T>` | 非泛型 `bool → Result`，或 `Result.Ok<bool>(true)` |
| `Result → string` 隐式转换 | `ToString()` |
| `string s = resultOfT;`（泛型） | `T=string` 时用 D-03；否则 `ToString()` |

---

## 8. 观察区（本期不写代码）

- `MAG-008b`：`ErrorMessages` / `FirstError` / `FirstException`
- `MAG-012`：`Dump` / `ToDebugString`
- `MAG-017`：`TryFunc(ref value, ...)`
- `MAG-018`：`Try` + `CancellationToken`
- `MAG-020`：`Previous` / `Trace`
- `MAG-021`：`Combine`
- `MAG-024`：`[JsonIgnore]` / 序列化调整（随 020/021）

---

## 9. 实现备注

| 项 | 位置 | 说明 |
| --- | --- | --- |
| `string → Result`、`Exception → Result`、`bool → Result`、`Result → bool/Exception` | `Result`（partial） | 转换内部统一走 `Fail(...)`，保证错误工厂生效 |
| `string/Exception → Result<T>`、`Result<T> → T/bool/Exception/Result` | `Result<T>`（改为 partial） | 保留类型二义性见第 4 节 |
| `ErrorMessage`、`Throw`、`ThrowIfFailed` | `ResultBase` | `Throw` 复用 `Result → Exception` 的解析逻辑 |
| `ToError` | 扩展类 | 仅 `string` / `Exception` 两个来源，各含泛型版本 |
| `DefaultErrorMessage` / `DefaultError` | `Result` | `const string` + 进程级 `Exception` 单例 |
| 测试 | `FluentResults.Test/MagNet*Tests.cs` | 每个 ID 至少一个 Fact；D 项补编译契约（正例可编译、反例注释） |

### ID → 测试命名约定

```text
MAG-001 -> MagNetConversionTests.String_literal_is_failure
MAG-002 -> MagNetConversionTests.Exception_literal_is_failure_and_keeps_instance
MAG-003 -> MagNetConversionTests.Bool_literal_maps_to_success_or_failure
MAG-004 -> MagNetConversionTests.Result_to_bool_returns_IsSuccess
MAG-006 -> MagNetConversionTests.Result_to_exception_prefers_exceptional_error
MAG-007 -> MagNetConversionTests.Value_result_to_plain_keeps_reasons
MAG-008a -> MagNetReadTests.ErrorMessage_joins_all_error_messages
MAG-010 -> MagNetThrowTests.ThrowIfFailed_throws_only_on_failure
MAG-013a -> MagNetConversionTests.ToError_extensions_create_failures
MAG-014 -> MagNetDefaultsTests.DefaultError_is_singleton
D-01..D-05 -> CompileContractTests.*（文档 + 探针）
```
