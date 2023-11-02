using CalendarAnalyser.Core.Configuration;

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

        var rules = YAMLDeserializer.Deserialize(fileContent);

        return builder.WithRules(rules.ToList());
    }
}