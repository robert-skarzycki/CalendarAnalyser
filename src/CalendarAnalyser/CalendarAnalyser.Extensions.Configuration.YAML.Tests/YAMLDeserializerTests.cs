using CalendarAnalyser.Core;

namespace CalendarAnalyser.Extensions.Configuration.YAML.Tests;

public class YAMLDeserializerTests
{
    [Fact]
    public void DeserializesRegexRuleType()
    {
        var content = @"
categories:
- name: SomeCategory
  rules:
  - type: regex
    pattern: Starting With.*
";

        var rules = YAMLDeserializer.Deserialize(content);

        rules.Count().Should().Be(1);

        rules.First().Should().BeOfType<RegexAnalysisRule>();
    }

    [Fact]
    public void DeserializesMultipleRulesWithinCategory()
    {
        var content = @"
categories:
- name: SomeCategory
  rules:
  - type: regex
    pattern: Starting With.*
  - type: regex
    pattern: .*Ending With
";

        var rules = YAMLDeserializer.Deserialize(content);

        rules.Count().Should().Be(2);

        rules.All(r => r.Category == "SomeCategory").Should().BeTrue();
    }
}