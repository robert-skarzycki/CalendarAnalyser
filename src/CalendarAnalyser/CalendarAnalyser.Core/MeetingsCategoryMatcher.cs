using CalendarAnalyser.Core.Configuration;
using System.Linq;

namespace CalendarAnalyser.Core;

public static class MeetingsCategoryMatcher
{
    public static string Match(Meeting meeting, CalendarAnalysisConfiguration configuration)
    {
        var matchingRules = configuration.Rules.Where(r => r.IsMatch(meeting)).ToArray();
        var matchingCategories = matchingRules.Select(r => r.Category).Distinct().ToArray();

        if (matchingCategories.Length > 1)
        {
            configuration.LogAction($"More than one rules apply: ${string.Join(", ", matchingCategories)} for \"${meeting.Subject}\" on ${meeting.StartDateTime:o}");
        }

        var categoryName = matchingCategories.Length == 0 ? Constants.OtherCategoryName : matchingCategories.First(); // TODO: Handle multiple categories matching

        return categoryName;
    }
}
