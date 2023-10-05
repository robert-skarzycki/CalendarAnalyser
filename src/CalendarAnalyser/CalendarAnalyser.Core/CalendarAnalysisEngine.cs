using CalendarAnalyser.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CalendarAnalyser.Core;

public class CalendarAnalysisEngine
{
    private const string OtherCategoryName = "Other";
    private readonly CalendarAnalysisConfiguration configuration;

    public CalendarAnalysisEngine(CalendarAnalysisConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public CalendarAnalysisResult Analyze(ICollection<Meeting> meetings)
    {
        if (meetings == null) { throw new ArgumentNullException(nameof(meetings)); }

        var totalDurationPerCategory = AnalyzeInCategories(meetings);

        var result = new CalendarAnalysisResult(totalDurationPerCategory);

        return result;
    }

    private Dictionary<string, TimeSpan> AnalyzeInCategories(IEnumerable<Meeting> meetings)
    {
        var categories = configuration.Rules.ToDictionary(r => r.Category, _ => TimeSpan.Zero);
        categories.Add(OtherCategoryName, TimeSpan.Zero);

        foreach(var meeting in meetings)
        {
            var coreHoursStart = meeting.StartDateTime.Date.Add(configuration.CoreHoursStartTime);
            var trimmedMeetingStartDateTime = meeting.StartDateTime < coreHoursStart ? coreHoursStart : meeting.StartDateTime;
            var coreHoursEnd = meeting.StartDateTime.Date.Add(configuration.CoreHoursEndTime);
            var trimmedMeetingEndDateTime = meeting.EndDateTime > coreHoursEnd ? coreHoursEnd : meeting.EndDateTime;

            var meetingDuration = trimmedMeetingEndDateTime - trimmedMeetingStartDateTime;

            var matchingRules = configuration.Rules.Where(r => r.IsMatch(meeting)).ToArray();

            if (matchingRules.Length > 1)
            {
                throw new InvalidOperationException("More than one rules apply");
            }
            else
            {
                var categoryName = matchingRules.Length == 0 ? OtherCategoryName : matchingRules.First().Category;

                categories[categoryName] += meetingDuration;
            }
        }

        return categories;
    }
}
