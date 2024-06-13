using Microsoft.AspNetCore.Components;
using Wryte.Code;
using Wryte.Components;
using Wryte.Models;
using Wryte.Services;

namespace Wryte.Pages
{
    public partial class Statistics
    {

        // What statistics do I want to show
        // For each novel:
        // The number of chapters
        // The number of scenes
        // The total number of words
        // The average number of words per scene/chapter

        // The number of chapters/scenes in the last day/week/month
        // The number of words per scene/chapter in the last day/week/month
        // Use chartjs to show results

        // Services

        [Inject]
        public DatabaseService Database { get; set; }

        // Fields

        private StatisticsHelper _helper;

        private List<ItemModel> _novels;

        // Events

        protected override async Task OnInitializedAsync()
        {
            await SetUp();
        }

        // Events - controls

        private async Task NovelChanged(ChangeEventArgs e)
        {
            if (e == null)
                return;

            if (e.Value == null)
            {
                await _helper.GetResults(_novels.Select(x => x.Id).ToArray());
            }
            else if (Guid.TryParse(e.Value.ToString(), out Guid id))
            {
                await _helper.GetResults(id);
            }

            StateHasChanged();
        }

        private async Task SetUp()
        {
            _novels = await Database.Get(DatabaseService.Novels);

            _helper = new StatisticsHelper(Database)
            {
                RollingResults = true
            };

            await _helper.GetResults(_novels.Select(x => x.Id).ToArray());
        }

        private Chart.ChartConfig GetChartConfig()
        {
            var labels = new List<string>();

            var values = new List<string>();

            foreach (var record in _helper.GetDailyStatistics())
            {
                labels.Add(record.Item1.ToString("MMM, dd"));
                values.Add(record.Item2.Words.ToString());
            }

            var dataset = new Dataset
            {
                Data = values.ToArray(),
                Label = "Words"
            };

            var animation = new Animation
            {
                Duration = 0,
                Easing = "",
            };

            return new ChartBuilder()
                .WithType(ChartType.Line)
                .WithAnimation(animation)
                .WithDataset(dataset)
                .WithLabels(labels)
                .Build();

        }
        
        private int GetWordsPerDayUsed(int totalWords, List<SessionModel> stats)
        {
            // Find the average for days in which the user used Movel
            int numberDaysUsed = 1; // Has to be at least 1 day (today);

            // Skip the first stat, it's accounted for by setting 
            // numberDaysUsed = 1 and simplifies the loops
            for (int i = 1; i < stats.Count; i++)
            {
                var start = stats[i].Start;
                var previous = stats[i - 1].End;

                if (start.Date != previous.Date)
                    numberDaysUsed++;
            }
            return totalWords / numberDaysUsed;
        }

        private int GetWordsPerDayForMonth(int totalWords)
        {
            // Get the number of days from the start of the month
            var startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            int numberDays = (DateTime.Now - startDate).Days;
            return totalWords / numberDays;
        }

        private int GetTotalWordsWritten(List<SessionModel> stats)
        {
            int totalWords = 0;

            stats.ForEach(x => totalWords += x.WordCountEnd - x.WordCountStart);
            
            return totalWords;
        }

        private TimeSpan GetTotalTime(List<SessionModel> stats)
        {
            var durations = new List<TimeSpan>();

            stats.ForEach(x => durations.Add(x.End - x.Start));

            return new TimeSpan(durations.Sum(d => d.Ticks));
        }

    }
}
