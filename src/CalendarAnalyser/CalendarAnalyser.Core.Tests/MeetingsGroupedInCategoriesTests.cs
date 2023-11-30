using CalendarAnalyser.Core.Configuration;
using FluentAssertions;
using System.Text.RegularExpressions;

namespace CalendarAnalyser.Core.Tests;

public class MeetingsGroupedInCategoriesTests
{
    [Fact]
    public void DistinctsCategoriesEvenMultipleRulesForSameCategory()
    {
        var meetings = new Meeting[0];
        var DummyCategoryName = "DummyCategory";
        var configuration = new CalendarAnalysisConfiguration
        {
            Rules = new IAnalysisRule[] { 
                new RegexAnalysisRule(new Regex("a"), DummyCategoryName), 
                new RegexAnalysisRule(new Regex("b"), DummyCategoryName),
                new RegexAnalysisRule(new Regex("c"), DummyCategoryName),
                new RegexAnalysisRule(new Regex("d"), "OtherCategory")
            }
        };

        var sut = new MeetingsGroupedInCategories(meetings, configuration);

        sut.Data.Keys.Should().HaveCount(3);
        sut.Data.Keys.Contains(DummyCategoryName).Should().BeTrue();
    }
}
