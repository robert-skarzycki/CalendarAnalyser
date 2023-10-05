using System.Text.RegularExpressions;

namespace CalendarAnalyser.Core;

public class RegexAnalysisRule : IAnalysisRule
{
    private readonly Regex regex;

    public RegexAnalysisRule(Regex regex, string categoryName)
    {
        this.regex = regex;
        Category = categoryName;
    }

    public string Category { get; init; }

    public bool IsMatch(Meeting meeting)
    {
        return regex.IsMatch(meeting.Subject);
    }
}
