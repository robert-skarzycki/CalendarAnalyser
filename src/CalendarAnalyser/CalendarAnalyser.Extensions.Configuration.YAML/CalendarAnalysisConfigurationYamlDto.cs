namespace CalendarAnalyser.Extensions.Configuration.YAML;

internal class AnalysisMappingDto
{
    public AnalysisCategory[] Categories { get; set; }
}

internal class AnalysisCategory
{
    public string Name { get; set; }
    public AnalysisRuleDto[] Rules { get; set; }
}

internal abstract class AnalysisRuleDto
{
    public string Type { get; set; }    
}

internal class RegexAnalysisRuleDto : AnalysisRuleDto
{
    public string Pattern { get; set; }
}

internal static class RuleTypes
{
    internal static readonly string Regex = "regex";
}