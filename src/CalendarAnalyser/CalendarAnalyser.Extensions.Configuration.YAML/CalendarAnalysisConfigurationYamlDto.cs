namespace CalendarAnalyser.Extensions.Configuration.YAML;

internal class CalendarAnalysisConfigurationYamlDto
{
    public bool OnlyWorkingDays { get; set; } = true;
    public TimeSpan TimeResolution { get; set; } = TimeSpan.FromMinutes(30);
    public TimeSpan CoreHoursStartTime { get; set; } = TimeSpan.FromHours(9);
    public TimeSpan CoreHoursEndTime { get; set; } = TimeSpan.FromHours(15);
    public DateTime AnalysisStartDate { get; set; }
    public DateTime AnalysisEndDate { get; set; }
}