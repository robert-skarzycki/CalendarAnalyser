using System;
using System.Collections.Generic;
using System.Linq;

namespace CalendarAnalyser.Core;

public class CalendarAnalysisResult: ICalendarAnalysisResult
{
    public ICalendarCategoriesAnalysisResult CategoriesAnalysis { get; set; }

    public IEnumerable<ICalendarResultSlot> CalendarSlots { get; set; }
}

public class CalendarCategoriesAnalysisResult: ICalendarCategoriesAnalysisResult
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


public interface ICalendarAnalysisResult
{
    public ICalendarCategoriesAnalysisResult CategoriesAnalysis { get; set; }

    public IEnumerable<ICalendarResultSlot> CalendarSlots { get; set; }
}

public interface ICalendarCategoriesAnalysisResult
{
    Dictionary<string, AnalyzedCategoryInfo> Categories { get; }
}

public interface ICalendarResultSlot
{
    DateTime SlotStartDateTime { get; }
    string Category { get; }
}
internal record CalendarResultSlot(DateTime SlotStartDateTime, string Category): ICalendarResultSlot;