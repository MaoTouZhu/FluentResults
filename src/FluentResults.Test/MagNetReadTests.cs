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
        public void ErrorMessage_returns_first_message_for_multiple_errors()
        {
            Result.Fail(new[] { "first", "second" }).ErrorMessage.Should().Be("first");
        }
    }
}
