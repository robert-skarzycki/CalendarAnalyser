using FluentAssertions;
using System.Text.RegularExpressions;

namespace CalendarAnalyser.Core.Tests;

public class RegexAnalysisRuleTests
{
    [Fact]
    public void WhenMeetingNameMatches_IsMatchReturnsTrue()
    {
        var sut = new RegexAnalysisRule(new Regex("Dum.*"), "some-category");

        var meeting = DummyMeetingHelper.Build("Dummy");

        var result = sut.IsMatch(meeting);

        result.Should().BeTrue();
    }

    [Fact]
    public void WhenMeetingNameDoesNotMatch_IsMatchReturnsFalse()
    {
        var sut = new RegexAnalysisRule(new Regex("Dum.*"), "some-category");

        var meeting = DummyMeetingHelper.Build("NotDummy");

        var result = sut.IsMatch(meeting);

        result.Should().BeTrue();
    }
}
