using CalendarAnalyser.Core;
using CalendarAnalyser.Core.Configuration;
using System.Text.RegularExpressions;
using YamlDotNet.Serialization;

namespace CalendarAnalyser.Extensions.Configuration.YAML;

public static class CalendarAnalysisConfigurationBuilderExtensions
{
    public static CalendarAnalysisConfigurationBuilder WithRulesFromFile(this CalendarAnalysisConfigurationBuilder builder, string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"YAML configuration file cannot be found under: {filePath}");
        }
        var fileContent = File.ReadAllText(filePath);

        var deserializer = new DeserializerBuilder()
            .WithTypeDiscriminatingNodeDeserializer(o =>
            {
                var valueMappings = new Dictionary<string, Type> 
                {
                    { RuleTypes.Regex, typeof(RegexAnalysisRuleDto) }
                };
                o.AddKeyValueTypeDiscriminator<AnalysisRuleDto>(nameof(AnalysisRuleDto.Type), valueMappings);
            })
            .Build();

        var dto = deserializer.Deserialize<AnalysisMappingDto>(fileContent);

        var rules = MaptDtoToRules(dto);

        return builder.WithRules(rules.ToList());
    }

    private static IEnumerable<IAnalysisRule> MaptDtoToRules(AnalysisMappingDto dto)
    {
        foreach(var category in dto.Categories)
        {
            foreach(var rule in category.Rules)
            {
                var analysisRule = rule switch
                {
                    RegexAnalysisRuleDto regexRule => new RegexAnalysisRule(new Regex(regexRule.Pattern), category.Name),
                    _ => throw new IndexOutOfRangeException("Invalid rule type")
                };

                yield return analysisRule;
            }
        }
    }
}
