using System;

namespace CalendarAnalyser.Core.Results;

public interface ICalendarResultSlot
{
    TimeOnly SlotStartDateTime { get; }
    string Category { get; }
}
