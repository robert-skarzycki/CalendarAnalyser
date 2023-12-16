using CalendarAnalyser.Core;
using CalendarAnalyser.Core.Results;
using SpreadCheetah;
using SpreadCheetah.Styling;
using SpreadCheetah.Worksheets;

namespace CalendarAnalyser.Extensions.Export.Excel
{
    public static class CalendarAnalyserExcelExporter
    {
        public static async Task Export(string filePath, ICalendarAnalysisResult result)
        {
            using var stream = File.Create(filePath);
            using var spreadsheet = await Spreadsheet.CreateNewAsync(stream);
            
            await BuildCategoryStatsWorksheeet(spreadsheet, result);

            await BuildSlotsWorksheet(spreadsheet, result);

            await spreadsheet.FinishAsync();
        }

        private static async Task BuildSlotsWorksheet(Spreadsheet spreadsheet, ICalendarAnalysisResult result)
        {
            var worksheetOptions = new WorksheetOptions();
            worksheetOptions.Column(1).Width = 25;
            worksheetOptions.Column(2).Width = 25;

            var headerStyle = new Style();
            headerStyle.Font.Bold = true;
            var headerStyleId = spreadsheet.AddStyle(headerStyle);

            foreach (var day in result.CalendarSlotsPerWorkingDay) 
            {
                await spreadsheet.StartWorksheetAsync(day.Key.ToString("ddd yyyy-MM-dd"), worksheetOptions);


                var headerRow = new List<Cell>
                {
                    new("Time", headerStyleId),
                    new("Category", headerStyleId)
                };
                await spreadsheet.AddRowAsync(headerRow);

                foreach (var slot in day.Value)
                {
                    var row = new List<Cell>
                    {
                        new(slot.SlotStartDateTime.ToString("HH:mm")),
                        new(string.Equals(slot.Category, Constants.FreeCategoryName) ? string.Empty : slot.Category)
                    };

                    await spreadsheet.AddRowAsync(row);
                }
            }
        }

        private static async Task BuildCategoryStatsWorksheeet(Spreadsheet spreadsheet, ICalendarAnalysisResult result)
        {
            var worksheetOptions = new WorksheetOptions();
            worksheetOptions.Column(1).Width = 25;
            worksheetOptions.Column(2).Width = 25;
            worksheetOptions.Column(3).Width = 25;

            await spreadsheet.StartWorksheetAsync("Category stats", worksheetOptions);

            var headerStyle = new Style();
            headerStyle.Font.Bold = true;
            var headerStyleId = spreadsheet.AddStyle(headerStyle);

            var headerRow = new List<Cell>
            {
                new("Category name", headerStyleId),
                new("Percentage", headerStyleId),
                new("Total duration", headerStyleId)
            };

            await spreadsheet.AddRowAsync(headerRow);

            foreach (var category in result.CategoriesAnalysis.Categories)
            {
                var row = new List<Cell>
                {
                    new(category.Key),
                    new(category.Value.Percentage),
                    new(category.Value.TotalDuration.ToString())
                };

                await spreadsheet.AddRowAsync(row);
            }
        }
    }
}