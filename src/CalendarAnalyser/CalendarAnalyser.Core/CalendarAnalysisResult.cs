using System;
using System.Collections.Generic;
using System.Linq;
using CalendarAnalyser.Core.Results;

namespace CalendarAnalyser.Core;

internal record CalendarAnalysisResult(ICalendarCategoriesAnalysisResult CategoriesAnalysis, Dictionary<DateOnly, IEnumerable<ICalendarResultSlot>> CalendarSlotsPerWorkingDay) : ICalendarAnalysisResult;

internal class CalendarCategoriesAnalysisResult: ICalendarCategoriesAnalysisResult
{
    public Dictionary<string, AnalyzedCategoryInfo> Categories { get; } = new Dictionary<string, AnalyzedCategoryInfo>();

    public CalendarCategoriesAnalysisResult(Dictionary<string, TimeSpan> totalDurationPerCategory)
    {
        var totalWorkingTime = TimeSpan.FromTicks(totalDurationPerCategory.Sum(c => c.Value.Ticks));

        foreach (var categoryPair in totalDurationPerCategory)
        {
            var categoryName = categoryPair.Key;
            var totalTimeInCategory = categoryPair.Value;

            Categories[categoryName] = new AnalyzedCategoryInfo(totalTimeInCategory, totalTimeInCategory.TotalMinutes / totalWorkingTime.TotalMinutes);
        }
    }
}

public record AnalyzedCategoryInfo(TimeSpan TotalDuration, double Percentage);
internal record CalendarResultSlot(TimeOnly SlotStartDateTime, string Category): ICalendarResultSlot;