using System.Collections.Generic;

namespace CalendarAnalyser.Core.Results;

public interface ICalendarCategoriesAnalysisResult
{
    Dictionary<string, AnalyzedCategoryInfo> Categories { get; }
}
