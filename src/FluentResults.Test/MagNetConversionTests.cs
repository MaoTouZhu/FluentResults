using System;
using FluentAssertions;
using Xunit;

namespace FluentResults.Test
{
    public class MagNetConversionTests
    {
        [Fact]
        public void String_literal_is_failure()
        {
            Result result = "boom";

            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle();
            result.Errors[0].Message.Should().Be("boom");
        }

        [Fact]
        public void Null_string_is_failure_with_default_message()
        {
            Result result = (string)null;

            result.IsFailed.Should().BeTrue();
            result.ErrorMessage.Should().Be(Result.DefaultErrorMessage);
        }

        [Fact]
        public void Exception_literal_is_failure_and_keeps_instance()
        {
            var exception = new InvalidOperationException("boom");

            Result result = exception;

            result.IsFailed.Should().BeTrue();
            result.Errors.Should().ContainSingle();
            result.Errors[0].Should().BeOfType<ExceptionalError>();
            ((IExceptionalError)result.Errors[0]).Exception.Should().BeSameAs(exception);
        }

        [Fact]
        public void Null_exception_is_failure_with_default_message()
        {
            Result result = (Exception)null;

            result.IsFailed.Should().BeTrue();
            result.ErrorMessage.Should().Be(Result.DefaultErrorMessage);
        }

        [Fact]
        public void Bool_maps_to_success_or_failure()
        {
            Result success = true;
            Result failure = false;

            success.IsSuccess.Should().BeTrue();
            failure.IsFailed.Should().BeTrue();
            failure.ErrorMessage.Should().Be(Result.DefaultErrorMessage);
        }

        [Fact]
        public void Result_supports_truthiness()
        {
            if (!Result.Ok())
                throw new InvalidOperationException("expected success result to be truthy");

            if (Result.Fail("boom"))
                throw new InvalidOperationException("expected failed result to be falsy");

            (!Result.Ok()).Should().BeFalse();
            (!Result.Fail("boom")).Should().BeTrue();
        }

        [Fact]
        public void String_literal_is_failure_for_value_result()
        {
            Result<int> result = "boom";

            result.IsFailed.Should().BeTrue();
            result.ErrorMessage.Should().Be("boom");
            result.ValueOrDefault.Should().Be(0);
        }

        [Fact]
        public void Null_string_is_failure_for_value_result()
        {
            Result<int> result = (string)null;

            result.IsFailed.Should().BeTrue();
            result.ErrorMessage.Should().Be(Result.DefaultErrorMessage);
            result.ValueOrDefault.Should().Be(0);
        }

        [Fact]
        public void Exception_literal_is_failure_for_value_result()
        {
            var exception = new InvalidOperationException("boom");

            Result<int> result = exception;

            result.IsFailed.Should().BeTrue();
            result.ValueOrDefault.Should().Be(0);
            result.Errors.Should().ContainSingle();
            ((IExceptionalError)result.Errors[0]).Exception.Should().BeSameAs(exception);
        }

        [Fact]
        public void Value_result_is_implicitly_convertible_to_value()
        {
            Result<int> success = Result.Ok(42);
            Result<int> failure = Result.Fail<int>("boom");

            int successValue = success;
            int failureValue = failure;

            successValue.Should().Be(42);
            failureValue.Should().Be(0);
        }

        [Fact]
        public void Value_result_supports_truthiness()
        {
            if (!Result.Ok(42))
                throw new InvalidOperationException("expected success result to be truthy");

            if (Result.Fail<int>("boom"))
                throw new InvalidOperationException("expected failed result to be falsy");

            (!Result.Ok(42)).Should().BeFalse();
            (!Result.Fail<int>("boom")).Should().BeTrue();
        }

        [Fact]
        public void Value_result_is_implicitly_convertible_to_exception()
        {
            var exception = new InvalidOperationException("boom");
            Result<int> failed = Result.Fail<int>(new ExceptionalError(exception));

            Exception fromResult = failed;

            fromResult.Should().BeSameAs(exception);
        }

        [Fact]
        public void Failed_value_result_without_exception_is_converted_to_exception_with_message()
        {
            Result<int> failed = Result.Fail<int>("boom");

            Exception fromResult = failed;

            fromResult.Message.Should().Be("boom");
        }

        [Fact]
        public void Successful_result_is_converted_to_default_error()
        {
            Exception fromResult = Result.Ok();

            fromResult.Should().BeSameAs(Result.DefaultError);
        }

        [Fact]
        public void Value_result_is_implicitly_convertible_to_plain_result()
        {
            Result<int> withValue = Result.Fail<int>("boom");

            Result plain = withValue;

            plain.IsFailed.Should().BeTrue();
            plain.Reasons.Should().ContainSingle();
            plain.Errors[0].Should().BeSameAs(withValue.Errors[0]);
        }

        [Fact]
        public void ToError_extensions_create_failures()
        {
            var exception = new InvalidOperationException("boom");

            Result fromString = "boom".ToError();
            Result<int> fromStringOfT = "boom".ToError<int>();
            Result fromException = exception.ToError();
            Result<int> fromExceptionOfT = exception.ToError<int>();

            fromString.ErrorMessage.Should().Be("boom");
            fromStringOfT.ErrorMessage.Should().Be("boom");
            fromStringOfT.ValueOrDefault.Should().Be(0);
            ((IExceptionalError)fromException.Errors[0]).Exception.Should().BeSameAs(exception);
            ((IExceptionalError)fromExceptionOfT.Errors[0]).Exception.Should().BeSameAs(exception);
        }

        [Fact]
        public void ToError_on_null_string_uses_default_message()
        {
            Result result = ((string)null).ToError();

            result.ErrorMessage.Should().Be(Result.DefaultErrorMessage);
        }

        [Fact]
        public void Bool_value_result_supports_truthiness_and_value_extraction()
        {
            // Result<bool> is no longer ambiguous: truthiness uses operator true/false,
            // value extraction uses the generic T conversion.
            Result<bool> result = true;

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeTrue();

            bool value = result;
            value.Should().BeTrue();

            if (!result)
                throw new InvalidOperationException("expected success result to be truthy");

            Result<bool> failed = Result.Fail<bool>("boom");
            if (failed)
                throw new InvalidOperationException("expected failed result to be falsy");

            bool failedValue = failed;
            failedValue.Should().BeFalse();
        }
    }
}
