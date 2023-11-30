namespace CalendarAnalyser.Core.Tests;

public record DummyMeeting(string Name) : Meeting(Name, new DateTime(2023, 8, 1, 9, 0, 0), new DateTime(2023, 8, 1, 9, 30, 0), 1, "john.smith@company.com", false, false);