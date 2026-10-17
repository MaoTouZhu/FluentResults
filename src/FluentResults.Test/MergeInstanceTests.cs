using System.Linq;
using FluentAssertions;
using Xunit;

namespace FluentResults.Test
{
    public class MergeInstanceTests
    {
        [Fact]
        public void Result_Merge_includes_this_and_given_results()
        {
            var merged = Result.Fail("first").Merge(Result.Fail("second"));

            merged.IsFailed.Should().BeTrue();
            merged.Errors.Select(error => error.Message).Should().Equal("first", "second");
        }

        [Fact]
        public void Result_Merge_accepts_value_results()
        {
            var merged = Result.Ok().Merge(Result.Ok(42), Result.Fail("boom"));

            merged.IsFailed.Should().BeTrue();
            merged.Errors.Should().ContainSingle().Which.Message.Should().Be("boom");
        }

        [Fact]
        public void Result_Merge_without_arguments_keeps_this()
        {
            var merged = Result.Fail("first").Merge();

            merged.IsFailed.Should().BeTrue();
            merged.Errors.Should().ContainSingle().Which.Message.Should().Be("first");
        }

        [Fact]
        public void ValueResult_Merge_returns_merged_value_list()
        {
            var merged = Result.Ok(1).Merge(Result.Ok(2), Result.Ok(3));

            merged.IsSuccess.Should().BeTrue();
            merged.Value.Should().Equal(1, 2, 3);
        }

        [Fact]
        public void ValueResult_Merge_propagates_errors()
        {
            var merged = Result.Ok(1).Merge(Result.Fail<int>("boom"));

            merged.IsFailed.Should().BeTrue();
            merged.Errors.Should().ContainSingle().Which.Message.Should().Be("boom");
        }

        [Fact]
        public void ValueResult_Merge_without_arguments_returns_single_value()
        {
            var merged = Result.Ok(42).Merge();

            merged.IsSuccess.Should().BeTrue();
            merged.Value.Should().Equal(42);
        }
    }
}
