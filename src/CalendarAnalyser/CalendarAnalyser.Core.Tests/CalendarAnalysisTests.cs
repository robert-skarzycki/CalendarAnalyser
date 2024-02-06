using CalendarAnalyser.Core.Configuration;
using FluentAssertions;
using System.Text.RegularExpressions;

namespace CalendarAnalyser.Core.Tests;

public class CalendarAnalysisTests
{
    [Fact]
    public void WhenNoRulesMatching_ShouldFallbackToOtherCategory()
    {
        var configuration = TestConfigurationBuilder()
            .WithRules(new[] { new RegexAnalysisRule(new Regex("NotDummy"), "NotDummy") })
            .Build();
        var meetings = new[] {
            DummyMeeting((10, 0),(10, 30)),
            DummyMeeting((11, 0),(11, 30))
        };
        var sut = new CalendarAnalysis(configuration, meetings);

        var result = sut.AnalyzeCategories();

        result.Categories["NotDummy"].TotalDuration.TotalMinutes.Should().Be(0, "no meeting matches");
        result.Categories[Constants.OtherCategoryName].TotalDuration.TotalMinutes.Should().Be(60, "total sum of all meetings is 60 mins");
    }

    [Fact]
    public void WhenRuleMatches_ShouldSumAllMatchingMeetings()
    {
        var configuration = TestConfigurationBuilder()
            .WithRules(new[] { new RegexAnalysisRule(new Regex("Dummy"), "Dummy") })
            .Build();

        var meetings = new[] {
            DummyMeeting((10, 0), (10, 30)),
            DummyMeeting((11, 0), (11, 30))
        };

        var sut = new CalendarAnalysis(configuration, meetings);

        var result = sut.AnalyzeCategories();

        result.Categories[Constants.OtherCategoryName].TotalDuration.TotalMinutes.Should().Be(0, "all meeting matches a rule");
        result.Categories["Dummy"].TotalDuration.TotalMinutes.Should().Be(60, "total sum of all meetings is 60 mins");
    }

    [Fact]
    public void WhenMeetingStartsEarlierThenCoreHours_ShouldBeTrimmed()
    {
        var configuration = TestConfigurationBuilder()
            .WithCoreHoursStartAt(TimeSpan.FromHours(8))
            .Build();

        var meetings = new[] {
            DummyMeeting((7, 0), (9, 0))
        };

        var sut = new CalendarAnalysis(configuration, meetings);

        var result = sut.AnalyzeCategories();

        result.Categories[Constants.OtherCategoryName].TotalDuration.TotalMinutes.Should().Be(60, "120 min meeting should be trimmed by 60 minnutes (only 8:00-9:00 counts)");
    }

    [Fact]
    public void WhenMeetingFullyOutsideCoreHours_ShouldNotBeCounted()
    {
        var configuration = TestConfigurationBuilder()
            .WithCoreHoursStartAt(TimeSpan.FromHours(8))
            .Build();

        var meetings = new[] {
            DummyMeeting((7, 0), (7, 30))
        };

        var sut = new CalendarAnalysis(configuration, meetings);

        var result = sut.AnalyzeCategories();

        result.Categories[Constants.OtherCategoryName].TotalDuration.TotalMinutes.Should().Be(0, "Meeeting at 7:00-8:00 should not counted - core hours start is at 8:00");
    }

    [Fact]
    public void WhenOnlyWorkingDays_WeekendShouldNotBeCountedInFreeTime()
    {
        var configuration = TestConfigurationBuilder()
            .WithCoreHoursStartAt(TimeSpan.FromHours(8))
            .WithCoreHoursEndAt(TimeSpan.FromHours(16))
            .WithOnlyWorkingDays()
            .Build();

        var meetings = new[] {
            DummyMeeting(7, 31, (8, 0), (16, 0)),
            DummyMeeting(8, 1, (8, 0), (16, 0)),
            DummyMeeting(8, 2, (8, 0), (16, 0)),
            DummyMeeting(8, 3, (8, 0), (16, 0)),
            DummyMeeting(8, 4, (8, 0), (16, 0)),
        };

        var sut = new CalendarAnalysis(configuration, meetings);

        var result = sut.AnalyzeCategories();

        result.Categories[Constants.OtherCategoryName].Percentage.Should().Be(0.5, "1 week full in meetings, other not, weekend - excluded");
        result.Categories[Constants.FreeCategoryName].Percentage.Should().Be(0.5, "1 week full in meetings, other not, weekend - excluded");
        result.Categories[Constants.FreeCategoryName].TotalDuration.TotalHours.Should().Be(40, "second week free, weekend - excluded");
    }

    private CalendarAnalysisConfigurationBuilder TestConfigurationBuilder()
    {
        return new CalendarAnalysisConfigurationBuilder().WithAnalysisDateRange(new DateTime(2023, 7, 31), new DateTime(2023, 8, 13));
    }

    private Meeting DummyMeeting((int, int) startTime, (int, int) endTime) => new("Dummy", new DateTime(2023, 8, 1, startTime.Item1, startTime.Item2, 0), new DateTime(2023, 8, 1, endTime.Item1, endTime.Item2, 0), 1, "john.smith@company.com", false, false);
    private Meeting DummyMeeting(int month, int day, (int, int) startTime, (int, int) endTime) => new("Dummy", new DateTime(2023, month, day, startTime.Item1, startTime.Item2, 0), new DateTime(2023, month, day, endTime.Item1, endTime.Item2, 0), 1, "john.smith@company.com", false, false);
}