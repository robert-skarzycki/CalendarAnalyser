using CalendarAnalyser.Core;
using SpreadCheetah;

namespace CalendarAnalyser.Extensions.Export.Excel
{
    public static class CalendarAnalyserExcelExporter
    {
        public static async Task Export(string filePath, CalendarAnalysisResult result)
        {
            using var stream = File.Create(filePath);
            using var spreadsheet = await Spreadsheet.CreateNewAsync(stream);
            
            await BuildCategoryStatsWorksheeet(spreadsheet, result);

            await BuildSlotsWorksheet(spreadsheet, result);

            await spreadsheet.FinishAsync();
        }

        private static async Task BuildSlotsWorksheet(Spreadsheet spreadsheet, CalendarAnalysisResult result)
        {
            var groupedSlots = result.CalendarSlots.GroupBy(s => s.SlotStartDateTime.Date);

            foreach(var day in groupedSlots) 
            {
                await spreadsheet.StartWorksheetAsync(day.Key.ToString("ddd yyyy-MM-dd"));

                foreach(var slot in day)
                {
                    var row = new List<Cell>
                    {
                        new Cell(slot.SlotStartDateTime.ToString()),
                        new Cell(slot.Category)
                    };

                    await spreadsheet.AddRowAsync(row);
                }
            }
        }

        private static async Task BuildCategoryStatsWorksheeet(Spreadsheet spreadsheet, CalendarAnalysisResult result)
        {
            await spreadsheet.StartWorksheetAsync("Category stats");

            var headerRow = new List<Cell>
            {
                new Cell("Category name"),
                new Cell("Percentage"),
                new Cell("Total duration")
            };

            await spreadsheet.AddRowAsync(headerRow);

            foreach (var category in result.CategoriesAnalysis.Categories)
            {
                var row = new List<Cell>
                {
                    new Cell(category.Key),
                    new Cell(category.Value.Percentage),
                    new Cell(category.Value.TotalDuration.ToString())
                };

                await spreadsheet.AddRowAsync(row);
            }
        }
    }
}