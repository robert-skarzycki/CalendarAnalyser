# CalendarAnalyser
This is a library for analysing someone's calendar - by categorizing provided list of meetings with configured rules. It maybe helpful to analyze working days - your or your employees - how much time is spent on different categories (at least diuring meetings).

## Basic usage
```csharp
var analyzer = new CalendarAnalysisConfigurationBuilder()
    .WithAnalysisDateRange(new DateTime(2023, 7, 31), new DateTime(2023, 8, 13))
    .WithRules(new[] { new RegexAnalysisRule(new Regex(".*ASAP.*"), "BusinessCritical") })
    .Build();

var meetings = new[]{
    new Meeting("We have to adjust plan ASAP", new DateTime(2023, 8, 1, 9, 30, 0), new DateTime(2023, 8, 1, 10, 0, 0), 1, "john.smith@company.com", false, false)
    // more meetings here...
};
var result = analyzer.Analyze(meetings);
```

Example above shows analysis based for specific timeframe (between 2023-07-31 and 2023-08-13) and with rule using regular expression pattern (meetings subject containg word _ASAP_ falls into category _BusinessCritical_). One meeting is provided in example, of course a collection of meetings is expected there.
There are some configuration options available like excluding weekends and specifying core hours (omitting meetings outside of them).