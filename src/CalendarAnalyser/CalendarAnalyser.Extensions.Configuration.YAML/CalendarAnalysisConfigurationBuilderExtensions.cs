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

        // TOOD: deserialize YAML with YAML dtos
        // map DTOs to actual rules

        return builder;
    }
}