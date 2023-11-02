using CalendarAnalyser.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CalendarAnalyser.Core;

public class CalendarAnalysisResult
{
    public CalendarCategoriesAnalysisResult CategoriesAnalysis { get; set; }

    public IEnumerable<CalendarSlot> CalendarSlots { get; set; }
}

public class CalendarCategoriesAnalysisResult
{
    public Dictionary<string, AnalyzedCategoryInfo> Categories { get; } = new Dictionary<string, AnalyzedCategoryInfo>();

    public CalendarCategoriesAnalysisResult(Dictionary<string, TimeSpan> totalDurationPerCategory, TimeSpan totalWorkingTime)
    {
        foreach (var categoryPair in totalDurationPerCategory)
        {
            var categoryName = categoryPair.Key;
            var totalTimeInCategory = categoryPair.Value;

            Categories[categoryName] = new AnalyzedCategoryInfo(totalTimeInCategory, totalTimeInCategory.TotalMinutes / totalWorkingTime.TotalMinutes);
        }

        var totalFreeTime = totalWorkingTime.TotalMinutes - Categories.Sum(c => c.Value.TotalDuration.TotalMinutes);

        Categories[Constants.FreeCategoryName] = new AnalyzedCategoryInfo(TimeSpan.FromMinutes(totalFreeTime), 1 - Categories.Sum(c => c.Value.Percentage));
    }
}

public record AnalyzedCategoryInfo(TimeSpan TotalDuration, double Percentage);

public record CalendarSlot(DateTime SlotStartDateTime, string Category);
    