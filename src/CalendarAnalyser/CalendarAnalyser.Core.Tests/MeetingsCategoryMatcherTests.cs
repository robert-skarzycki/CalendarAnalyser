﻿using CalendarAnalyser.Core.Configuration;
using FluentAssertions;
using System.Text.RegularExpressions;

namespace CalendarAnalyser.Core.Tests;

public class MeetingsCategoryMatcherTests
{
    private readonly CalendarAnalysisConfiguration configuration;
    private const string DummyCategoryName = "DummyCategory";

    public MeetingsCategoryMatcherTests()
    {
        configuration = new CalendarAnalysisConfigurationBuilder().WithRules
        (new IAnalysisRule[] {
                new RegexAnalysisRule(new Regex("Dummy.*"), DummyCategoryName)
            }
        ).Build();
    }

    [Fact]
    public void WhenRuleMatches_ReturnsCategoryName()
    {
        var result = MeetingsCategoryMatcher.Match(DummyMeetingHelper.Build("DummyMeeting"), configuration);

        result.Should().Be(DummyCategoryName);        
    }

    [Fact]
    public void WhenNoRuleMatches_ReturnsOtherCategoryName()
    {
        var result = MeetingsCategoryMatcher.Match(DummyMeetingHelper.Build("SomeMeeting"), configuration);

        result.Should().Be(Constants.OtherCategoryName);
    }
}
