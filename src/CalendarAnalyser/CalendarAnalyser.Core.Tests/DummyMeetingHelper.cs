namespace CalendarAnalyser.Core.Tests;

public static class DummyMeetingHelper
{
    public static Meeting Build((int, int) startTime, (int, int) endTime) => new("Dummy", new DateTime(2023, 8, 1, startTime.Item1, startTime.Item2, 0), new DateTime(2023, 8, 1, endTime.Item1, endTime.Item2, 0), 1, "john.smith@company.com", false, false);
    public static Meeting Build(int month, int day, (int, int) startTime, (int, int) endTime) => new("Dummy", new DateTime(2023, month, day, startTime.Item1, startTime.Item2, 0), new DateTime(2023, month, day, endTime.Item1, endTime.Item2, 0), 1, "john.smith@company.com", false, false);
    public static Meeting Build(string name) => new(name, new DateTime(2023, 8, 1, 9, 0, 0), new DateTime(2023, 8, 1, 9, 30, 0), 1, "john.smith@company.com", false, false);
}

