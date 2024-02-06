using CalendarAnalyser.Core.Configuration;
using CalendarAnalyser.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CalendarAnalyser.Core;

public class CalendarAnalysis
{
    private readonly CalendarAnalysisConfiguration configuration;
    private readonly List<WorkingDay> workingDays;
    private readonly Dictionary<string, List<string>> meetingNamesPerCategory;

    public CalendarAnalysis(CalendarAnalysisConfiguration configuration, ICollection<Meeting> meetings)
    {
        this.configuration = configuration;

        if (meetings == null) { throw new ArgumentNullException(nameof(meetings)); }

        var filteredMeetings = meetings.Where(ShouldMeetingBeIncluded).ToList();

        workingDays = BuildWorkingDays().ToList();

        AddMeetingsToDays(filteredMeetings);

        meetingNamesPerCategory = BuildMeetingNamesPerCategoryDictionary();
    }
    
    public ICalendarCategoriesAnalysisResult AnalyzeCategories()
    {
        var totalDurationPerCategory = CalculateTotalDurationPerCategory(workingDays);

        var categoriesAnalysisResult = new CalendarCategoriesAnalysisResult(totalDurationPerCategory);

        return categoriesAnalysisResult;
    }

    public Dictionary<DateOnly, IEnumerable<ICalendarResultSlot>> BuildCalendarSlotsPerDay()
    {
        ICalendarResultSlot ConvertWorkingDaySlotToResultSlot(CalendarSlot s) => new CalendarResultSlot(s.StartTime, string.Join(";", s.Categories));

        var calendarSlotsPerDay = workingDays.ToDictionary(wd => wd.Date, wd => wd.Slots.Select(ConvertWorkingDaySlotToResultSlot));

        return calendarSlotsPerDay;
    }

    public Dictionary<DateOnly, int> FindFocusSpotsPerDay()
    {
        int CountFocusSpotsInDay(WorkingDay workingDay)
        {
            var focusSpotsCount = 0;
            var focusSpotCandidateLength = 0;
            foreach(var slot in workingDay.Slots)
            {
                if(slot.Categories.Count == 1 && slot.Categories.First() == Constants.FreeCategoryName)
                {
                    focusSpotCandidateLength++;
                    if(focusSpotCandidateLength == configuration.FocusSpotSlotsNumberLength)
                    {
                        focusSpotCandidateLength = 0;
                        focusSpotsCount++;
                    }
                }
            }

            return focusSpotsCount;
        }

        var focusSpotsPerDay = workingDays.ToDictionary(wd => wd.Date, CountFocusSpotsInDay);

        return focusSpotsPerDay;
    }

    public Dictionary<string,IEnumerable<string>> DumpMeetingSubjectPerCategory()
    {
        return this.meetingNamesPerCategory.ToDictionary(pair => pair.Key, pair => pair.Value as IEnumerable<string>);
    }

    private Dictionary<string, List<string>> BuildMeetingNamesPerCategoryDictionary()
    {
        var result = configuration.Rules.Select(r => r.Category).Distinct().ToDictionary(c => c, _ => new List<string>());
        result.Add(Constants.OtherCategoryName, []);
        result.Add(Constants.FreeCategoryName, []);

        return result;
    }

    private bool ShouldMeetingBeIncluded(Meeting meeting)
    {
        if (configuration.FilterOutAllDayEvents && meeting.IsAllDay) { 
            return false; 
        }
        
        var isLastingMoreThanOneDay = meeting.StartDateTime.Date != meeting.EndDateTime.Date;
        
        if (isLastingMoreThanOneDay) { 
            return false;
        }

        return true;
    }

    private void AddMeetingsToDays(ICollection<Meeting> meetings)
    {
        foreach (var meeting in meetings)
        {
            var category = MeetingsCategoryMatcher.Match(meeting, configuration);

            if (configuration.CollectMeetingsPerCategory)
            {
                AddMeetingToCategory(category, meeting);
            }
            
            var meetingWorkingDay = workingDays.FirstOrDefault(wd => wd.Date == DateOnly.FromDateTime(meeting.StartDateTime));
            meetingWorkingDay?.AddMeetingWithCategory(meeting, category);
        }
    }

    private void AddMeetingToCategory(string category, Meeting meeting)
    {
        if (meetingNamesPerCategory.ContainsKey(category))
        {
            meetingNamesPerCategory[category].Add(meeting.Subject);
        }
        else
        {
            meetingNamesPerCategory[category] = [meeting.Subject];
        }
    }

    private IEnumerable<WorkingDay> BuildWorkingDays()
    {
        static bool IsWeekend(DateTime date) => date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;

        var currentDate = configuration.AnalysisStartDate.Date;

        while(currentDate < configuration.AnalysisEndDate.Date)
        {
            if(!configuration.OnlyWorkingDays || !IsWeekend(currentDate))
            {
                yield return new WorkingDay(DateOnly.FromDateTime(currentDate), configuration.TimeResolution, configuration.CoreHoursStartTime, configuration.CoreHoursEndTime);                               
            }

            currentDate = currentDate.AddDays(1);
        }
    }

    private Dictionary<string, TimeSpan> CalculateTotalDurationPerCategory(ICollection<WorkingDay> workingDays)
    {
        var result = configuration.Rules.Select(r => r.Category).Distinct().ToDictionary(category => category, _ => TimeSpan.Zero);
        result.Add(Constants.OtherCategoryName, TimeSpan.Zero);
        result.Add(Constants.FreeCategoryName, TimeSpan.Zero);

        void AddToCategory(string category, TimeSpan duration)
        {
            result[category] = result[category].Add(duration);
        }

        foreach (var workingDay in workingDays)
        {
            foreach (var slot in workingDay.Slots)
            {
                if (slot.Categories.Count == 1)
                {
                    AddToCategory(slot.Categories.First(), configuration.TimeResolution);
                }
                else
                {
                    var durationSplit = configuration.TimeResolution / slot.Categories.Count;
                    foreach (var categoryInSlot in slot.Categories)
                    {
                        AddToCategory(categoryInSlot, durationSplit);
                    }
                }
            }
        }

        return result;
    }
}
