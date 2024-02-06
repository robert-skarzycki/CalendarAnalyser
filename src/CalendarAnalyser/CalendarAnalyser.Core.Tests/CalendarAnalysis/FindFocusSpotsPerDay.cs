using CalendarAnalyser.Core.Configuration;
using FluentAssertions;
using System.Text.RegularExpressions;

namespace CalendarAnalyser.Core.Tests;
public partial class CalendarAnalysisTests
{
    public class FindFocusSpotsPerDay
    {
        [Fact]
        public void WhenAllSlotsAreBusy_Returns0()
        {
            var configuration =
                new CalendarAnalysisConfigurationBuilder()
                .WithAnalysisDateRange(new DateTime(2023, 8, 1), new DateTime(2023, 8, 2))                
                .WithRules(new[] { new RegexAnalysisRule(new Regex("Dummy"), "Dummy") })                   
                .Build();

            var meetings = new[] {
                DummyMeetingHelper.Build((9, 0),(12, 0)),
                DummyMeetingHelper.Build((12, 0),(15, 0))
            };
            var sut = new CalendarAnalysis(configuration, meetings);

            var result = sut.FindFocusSpotsPerDay();

            result[new DateOnly(2023, 8, 1)].Should().Be(0, "all slots are occupied with meeting");            
        }

        [Fact]
        public void ShouldNotCountTooSmallSpots()
        {
            var configuration =
                new CalendarAnalysisConfigurationBuilder()
                .WithAnalysisDateRange(new DateTime(2023, 8, 1), new DateTime(2023, 8, 2))
                .WithRules(new[] { new RegexAnalysisRule(new Regex("Dummy"), "Dummy") })
                .Build();

            var meetings = new[] {
                DummyMeetingHelper.Build((9, 0),(10, 30)),
                DummyMeetingHelper.Build((12, 0),(15, 0))
            };
            var sut = new CalendarAnalysis(configuration, meetings);

            var result = sut.FindFocusSpotsPerDay();

            result[new DateOnly(2023, 8, 1)].Should().Be(0, "there are 3 slots free when 4 are need to build spot");
        }

        [Fact]
        public void ShouldCountAdjacentSpots()
        {
            var configuration =
                new CalendarAnalysisConfigurationBuilder()
                .WithAnalysisDateRange(new DateTime(2023, 8, 1), new DateTime(2023, 8, 2))
                .WithRules(new[] { new RegexAnalysisRule(new Regex("Dummy"), "Dummy") })
                .Build();

            var meetings = new[] {
                DummyMeetingHelper.Build((9, 0),(10, 0)),
                DummyMeetingHelper.Build((14, 0),(15, 0))
            };
            var sut = new CalendarAnalysis(configuration, meetings);

            var result = sut.FindFocusSpotsPerDay();

            result[new DateOnly(2023, 8, 1)].Should().Be(2, "there are two spots: 10:00-12:00 and 12:00-14:00");
        }

        [Fact]
        public void ShouldCountSeparateSpots()
        {
            var configuration =
                new CalendarAnalysisConfigurationBuilder()
                .WithAnalysisDateRange(new DateTime(2023, 8, 1), new DateTime(2023, 8, 2))
                .WithRules(new[] { new RegexAnalysisRule(new Regex("Dummy"), "Dummy") })
                .Build();

            var meetings = new[] {
                DummyMeetingHelper.Build((9, 0),(10, 0)),
                DummyMeetingHelper.Build((12, 0),(13, 0))
            };
            var sut = new CalendarAnalysis(configuration, meetings);

            var result = sut.FindFocusSpotsPerDay();

            result[new DateOnly(2023, 8, 1)].Should().Be(2, "there are two spots: 10:00-12:00 and 13:00-15:00");
        }
    }
}