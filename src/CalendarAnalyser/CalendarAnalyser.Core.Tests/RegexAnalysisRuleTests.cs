using FluentAssertions;
using System.Text.RegularExpressions;

namespace CalendarAnalyser.Core.Tests;

public class RegexAnalysisRuleTests
{
    [Fact]
    public void WhenMeetingNameMatches_IsMatchReturnsTrue()
    {
        var sut = new RegexAnalysisRule(new Regex("Dum.*"), "some-category");

        var meeting = DummyMeeting("Dummy");

        var result = sut.IsMatch(meeting);

        result.Should().BeTrue();
    }

    [Fact]
    public void WhenMeetingNameDoesNotMatch_IsMatchReturnsFalse()
    {
        var sut = new RegexAnalysisRule(new Regex("Dum.*"), "some-category");

        var meeting = DummyMeeting("NotDummy");

        var result = sut.IsMatch(meeting);

        result.Should().BeTrue();
    }

    private Meeting DummyMeeting(string name) => new(name, new DateTime(2023, 8, 1, 9,0, 0), new DateTime(2023, 8, 1, 9, 30, 0), 1, "john.smith@company.com", false, false);
}
