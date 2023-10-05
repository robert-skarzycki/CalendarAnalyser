﻿namespace CalendarAnalyser.Core;

public interface IAnalysisRule
{
    bool IsMatch(Meeting meeting);

    string Category { get; }
}
