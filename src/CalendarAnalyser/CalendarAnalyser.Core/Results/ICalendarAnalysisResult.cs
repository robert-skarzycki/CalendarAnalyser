using System;
using System.Collections.Generic;

namespace CalendarAnalyser.Core.Results;

public interface ICalendarAnalysisResult
{
    public ICalendarCategoriesAnalysisResult CategoriesAnalysis { get; }

    public Dictionary<DateOnly, IEnumerable<ICalendarResultSlot>> CalendarSlotsPerWorkingDay { get; }
}
