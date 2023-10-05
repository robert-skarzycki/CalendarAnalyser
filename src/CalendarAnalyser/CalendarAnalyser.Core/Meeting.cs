using System;

namespace CalendarAnalyser.Core;

public record Meeting(string Subject, DateTime StartDateTime, DateTime EndDateTime, int AttendeesCount, string Organizer, bool IsAllDay, bool IsSelfOrganized);
