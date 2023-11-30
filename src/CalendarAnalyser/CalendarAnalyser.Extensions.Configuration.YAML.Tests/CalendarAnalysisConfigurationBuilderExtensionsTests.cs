using CalendarAnalyser.Core.Configuration;

namespace CalendarAnalyser.Extensions.Configuration.YAML.Tests;

public class CalendarAnalysisConfigurationBuilderExtensionsTests
{
    [Fact]
    public void ReadsRulesFromFile()
    {
        var configuration = new CalendarAnalysisConfigurationBuilder()
            .WithRulesFromFile("test-rules.yaml")
            .Build();

        configuration.Rules.Should().HaveCount(1);
    }
}