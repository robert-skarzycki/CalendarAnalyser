using CalendarAnalyser.Core.Configuration;
using FluentAssertions;
using System.Text.RegularExpressions;

namespace CalendarAnalyser.Core.Tests;

public class CalendarAnalysisEngineTests
{
    [Fact]
    public void WhenNoRulesMatching_ShouldFallbackToOtherCategory()
    {
        var configuration = new CalendarAnalysisConfigurationBuilder()
            .WithRules(new[] { new RegexAnalysisRule(new Regex("NotDummy"), "NotDummy") })
            .Build();
        
        var sut = new CalendarAnalysisEngine(configuration);

        var result = sut.Analyze(new[] { 
            new DummyMeeting((10, 0), (10, 30)),
            new DummyMeeting((11, 0), (11, 30))
        });

        result.TotalDurationPerCategory["NotDummy"].TotalMinutes.Should().Be(0, "no meeting matches");
        result.TotalDurationPerCategory["Other"].TotalMinutes.Should().Be(60, "total sum of all meetings is 60 mins");
    }

    [Fact]
    public void WhenRuleMatches_ShouldSumAllMatchingMeetings()
    {
        var configuration = new CalendarAnalysisConfigurationBuilder()
            .WithRules(new[] { new RegexAnalysisRule(new Regex("Dummy"), "Dummy") })
            .Build();

        var sut = new CalendarAnalysisEngine(configuration);

        var result = sut.Analyze(new[] {
            new DummyMeeting((10, 0), (10, 30)),
            new DummyMeeting((11, 0), (11, 30))
        });

        result.TotalDurationPerCategory["Other"].TotalMinutes.Should().Be(0, "all meeting matches a rule");
        result.TotalDurationPerCategory["Dummy"].TotalMinutes.Should().Be(60, "total sum of all meetings is 60 mins");
    }
}

public record DummyMeeting((int, int) StartTime, (int, int)EndTime) : Meeting("Dummy", new DateTime(2023, 8, 1, StartTime.Item1, StartTime.Item2, 0), new DateTime(2023, 8, 1, EndTime.Item1, EndTime.Item2, 0), 1, "john.smith@company.com", false, false);