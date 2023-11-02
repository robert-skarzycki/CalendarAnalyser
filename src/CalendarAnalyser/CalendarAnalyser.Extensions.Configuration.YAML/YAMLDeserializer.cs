using CalendarAnalyser.Core;
using System.Text.RegularExpressions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace CalendarAnalyser.Extensions.Configuration.YAML;

internal static class YAMLDeserializer
{
    public static IEnumerable<IAnalysisRule> Deserialize(string yamlContent)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithTypeDiscriminatingNodeDeserializer(o =>
            {
                var valueMappings = new Dictionary<string, Type>
                {
                    { RuleTypes.Regex, typeof(RegexAnalysisRuleDto) }
                };
                o.AddKeyValueTypeDiscriminator<AnalysisRuleDto>("type", valueMappings);
            })
            .Build();

        var dto = deserializer.Deserialize<AnalysisMappingDto>(yamlContent);

        var rules = MaptDtoToRules(dto);

        return rules;
    }

    private static IEnumerable<IAnalysisRule> MaptDtoToRules(AnalysisMappingDto dto)
    {
        foreach (var category in dto.Categories)
        {
            foreach (var rule in category.Rules)
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
