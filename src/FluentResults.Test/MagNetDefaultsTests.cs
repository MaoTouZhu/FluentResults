using FluentAssertions;
using Xunit;

namespace FluentResults.Test
{
    public class MagNetDefaultsTests
    {
        [Fact]
        public void DefaultErrorMessage_is_undefined_error()
        {
            Result.DefaultErrorMessage.Should().Be("Undefined error");
        }

        [Fact]
        public void DefaultError_is_singleton()
        {
            Result.DefaultError.Should().BeSameAs(Result.DefaultError);
        }
    }
}
