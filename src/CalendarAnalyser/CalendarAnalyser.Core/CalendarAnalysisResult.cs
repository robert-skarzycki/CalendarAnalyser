using System;
using System.Collections.Generic;

namespace CalendarAnalyser.Core;

public record CalendarAnalysisResult(Dictionary<string, TimeSpan> TotalDurationPerCategory);