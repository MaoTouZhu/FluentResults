using FluentAssertions;
using Xunit;

namespace FluentResults.Test
{
    public class MagNetReadTests
    {
        [Fact]
        public void ErrorMessage_returns_null_for_success()
        {
            Result.Ok().ErrorMessage.Should().BeNull();
            Result.Ok(42).ErrorMessage.Should().BeNull();
        }

        [Fact]
        public void ErrorMessage_returns_first_error_message()
        {
            Result.Fail("boom").ErrorMessage.Should().Be("boom");
            Result.Fail<int>("boom").ErrorMessage.Should().Be("boom");
        }

        [Fact]
        public void ErrorMessage_joins_all_error_messages()
        {
            Result.Fail(new[] { "first", "second" }).ErrorMessage.Should().Be("first; second");
            Result.Fail<int>(new[] { "first", "second" }).ErrorMessage.Should().Be("first; second");
            Result.Fail("first").WithError("second").ErrorMessage.Should().Be("first; second");
        }
    }
}
