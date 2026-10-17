using System;
using FluentAssertions;
using Xunit;

namespace FluentResults.Test
{
    public class MagNetThrowTests
    {
        [Fact]
        public void ThrowIfFailed_does_nothing_on_success()
        {
            Action act = () => Result.Ok().ThrowIfFailed();

            act.Should().NotThrow();
        }

        [Fact]
        public void ThrowIfFailed_throws_for_plain_error()
        {
            Action act = () => Result.Fail("boom").ThrowIfFailed();

            act.Should().Throw<Exception>().WithMessage("boom");
        }

        [Fact]
        public void ThrowIfFailed_rethrows_original_exception()
        {
            var exception = new InvalidOperationException("boom");

            Action act = () => Result.Fail(new ExceptionalError(exception)).ThrowIfFailed();

            act.Should().Throw<InvalidOperationException>().Which.Should().BeSameAs(exception);
        }

        [Fact]
        public void Throw_throws_default_error_for_success()
        {
            Action act = () => Result.Ok().Throw();

            act.Should().Throw<Exception>().Which.Should().BeSameAs(Result.DefaultError);
        }

        [Fact]
        public void Throw_statement_works_on_failed_result()
        {
            Result failed = Result.Fail("boom");

            Action act = () => throw failed;

            act.Should().Throw<Exception>().WithMessage("boom");
        }

        [Fact]
        public void Throw_statement_works_on_failed_value_result()
        {
            var exception = new InvalidOperationException("boom");
            Result<int> failed = Result.Fail<int>(new ExceptionalError(exception));

            Action act = () => throw failed;

            act.Should().Throw<InvalidOperationException>().Which.Should().BeSameAs(exception);
        }
    }
}
