using CalendarAnalyser.Core.Configuration;
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

        var deserializer = new DeserializerBuilder().Build();

        var dto = deserializer.Deserialize<CalendarAnalysisConfigurationYamlDto>(fileContent);

        builder = builder
            .WithTimeResolution(dto.TimeResolution)
            .WithAnalysisDateRange(dto.AnalysisStartDate, dto.AnalysisEndDate)
            .WithCoreHoursStartAt(dto.CoreHoursStartTime)
            .WithCoreHoursEndAt(dto.CoreHoursEndTime);            

        if (dto.OnlyWorkingDays)
        {
            builder = builder.WithOnlyWorkingDays();
        }

        return builder;
    }
}
