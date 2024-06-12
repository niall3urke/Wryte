using System.Collections.Generic;
using Wryte.Models;
using Wryte.Services;

namespace Wryte.Code
{
    public class StatisticsHelper
    {

        public List<SessionModel> Sessions { get; set; } = new List<SessionModel>();

        public WordAverage Average { get; set; } = new WordAverage();

        // Properties - words written in the last...

        public Statistic LastMonth { get; set; } = new Statistic();

        public Statistic LastYear { get; set; } = new Statistic();

        public Statistic LastWeek { get; set; } = new Statistic();

        public Statistic LastDay { get; set; } = new Statistic();

        public Statistic AllTime { get; set; } = new Statistic();

        // Properties - average words written across all days 

        public Statistic AverageMonthly { get; set; } = new Statistic();

        public Statistic AverageYearly { get; set; } = new Statistic();

        public Statistic AverageWeekly { get; set; } = new Statistic();

        public Statistic AverageDaily { get; set; } = new Statistic();

        // Properties 

        public bool AveragesEstimated { get; private set; }

        public bool RollingResults { get; set; }

        // Fields

        private readonly DatabaseService _db;

        // Constructors

        public StatisticsHelper(DatabaseService db)
        {
            _db = db;
        }

        // Methods

        private void Initialize()
        {
            Sessions = new List<SessionModel>();

            Average = new WordAverage();


            LastMonth = new Statistic();

            LastYear = new Statistic();

            LastWeek = new Statistic();

            LastDay = new Statistic();

            AllTime = new Statistic();

            // Averages

            AverageMonthly = new Statistic();

            AverageYearly = new Statistic();

            AverageWeekly = new Statistic();

            AverageDaily = new Statistic();

        }

        public async Task GetResults(params Guid[] ids)
        {
            Initialize();

            foreach (var id in ids)
            {
                Sessions.AddRange(await _db.GetNovelSessions(id));
            }

            SetStatistics(Sessions);

            SetAverages(Sessions);
        }

        public List<(DateTime, Statistic)> GetDailyStatistics()
        {
            var days = new List<(DateTime, Statistic)>();

            var dayQuartiles = new List<(DayOfWeek, TimeOfDay, Statistic)>();

            foreach (var yearGroup in Sessions.GroupBy(x => x.End.Year))
            {
                int year = yearGroup.First().End.Year;

                foreach (var monthGroup in yearGroup.ToList().GroupBy(x => x.End.Month))
                {
                    int month = monthGroup.First().End.Month;

                    foreach (var dayGroup in monthGroup.ToList().GroupBy(x => x.End.Day))
                    {
                        int day = dayGroup.First().End.Day;

                        var statistic = GetStatistic(dayGroup.ToList());

                        var date = new DateTime(year, month, day);

                        days.Add((date, statistic));
                    }
                }
            }

            return days;
        }

        public TimeOfDay GetMostProductiveTimeOfDay()
        {
            var days = new List<(TimeOfDay, DateTime, SessionModel)>();

            foreach (var yearGroup in Sessions.GroupBy(x => x.End.Year))
            {
                int year = yearGroup.First().End.Year;

                foreach (var monthGroup in yearGroup.ToList().GroupBy(x => x.End.Month))
                {
                    int month = monthGroup.First().End.Month;

                    foreach (var dayGroup in monthGroup.ToList().GroupBy(x => x.End.Day))
                    {
                        int day = dayGroup.First().End.Day;

                        foreach (var session in dayGroup.ToList())
                        {
                            TimeOfDay tod = TimeOfDay.Night;

                            if (session.End.Hour > 6 && session.End.Hour < 12)
                            {
                                tod = TimeOfDay.Morning;
                            }
                            else if (session.End.Hour > 11 && session.End.Hour < 17)
                            {
                                tod = TimeOfDay.Afternoon;
                            }
                            else if (session.End.Hour > 16 && session.End.Hour < 20)
                            {
                                tod = TimeOfDay.Evening;
                            }

                            var date = new DateTime(year, month, day);

                            days.Add((tod, date, session));

                            int wordsWritten = session.WordCountEnd - session.WordCountStart;

                        }
                    }
                }
            }

            TimeOfDay mostProductiveTime = TimeOfDay.Morning;

            int maxWordCount = int.MinValue;

            foreach (var timeOfDayGroup in days.GroupBy(x => x.Item1))
            {
                var sessions = timeOfDayGroup.ToList().Select(x => x.Item3).ToList();

                var statistic = GetStatistic(sessions);


                if (statistic.Words > maxWordCount)
                {
                    mostProductiveTime = timeOfDayGroup.First().Item1;

                    maxWordCount = statistic.Words;
                }
            }

            return mostProductiveTime;
        }

        public DayOfWeek GetMostProductDayOfWeek()
        {
            var days = new List<(DayOfWeek, Statistic)>();

            foreach (var dayOfWeekGroup in Sessions.GroupBy(x => x.End.DayOfWeek))
            {
                var statistic = GetStatistic(dayOfWeekGroup.ToList());

                days.Add((dayOfWeekGroup.Key, statistic));
            }
            
            if (days.Count == 0)
            {
                if (Sessions.Count > 0)
                {
                    return Sessions.First().End.DayOfWeek;
                }
                return DayOfWeek.Monday;
            }

            var orderedByWords = days.OrderByDescending(x => x.Item2.Words);

            return orderedByWords.First().Item1;
        }

        public enum TimeOfDay 
        {
            Morning, 
            Afternoon, 
            Evening,
            Night
        }

        // Methods: private

        private DateCutoff GetDateLimits()
        {
            var limit = new DateCutoff();

            if (RollingResults)
            {
                int weekOffset = (7 + (DateTime.Now.Date.DayOfWeek - DayOfWeek.Monday)) % 7;

                limit.Year = new DateTime(DateTime.Today.Year, 1, 1);

                limit.Month = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

                limit.Week = DateTime.Now.AddDays(-1 * weekOffset);

                limit.Day = DateTime.Today;
            }
            else
            {
                limit.Month = DateTime.Now.AddDays(-30);

                limit.Year = DateTime.Now.AddDays(-365);

                limit.Week = DateTime.Now.AddDays(-7);

                limit.Day = DateTime.Now.AddDays(-1);
            }

            return limit;
        }

        private void SetStatistics(List<SessionModel> sessions)
        {
            var cutoff = GetDateLimits();

            var recordsMonth = sessions.Where(x => x.Start >= cutoff.Month).ToList();

            var recordsWeek = sessions.Where(x => x.Start >= cutoff.Week).ToList();

            var recordsYear = sessions.Where(x => x.Start >= cutoff.Year).ToList();

            var recordsDay = sessions.Where(x => x.Start >= cutoff.Day).ToList();

            LastMonth = GetStatistic(recordsMonth);

            LastWeek = GetStatistic(recordsWeek);

            LastYear = GetStatistic(recordsYear);

            LastDay = GetStatistic(recordsDay);

            AllTime = GetStatistic(sessions);
        }

        private void SetAverages(List<SessionModel> sessions, bool writingDaysOnly = false)
        {
            var a = sessions.OrderBy(x => x.Start).FirstOrDefault();

            var b = sessions.OrderByDescending(x => x.Start).FirstOrDefault();

            if (a == null || b == null)
                return;

            var ts = b.End - a.Start;

            double years = Math.Max(ts.Days, 1) / 365.25;

            Average.Year = (int)(AllTime.Words / years);

            Average.Month = (int)(AllTime.Words / (years * 12));

            Average.Week = (int)(AllTime.Words / (years * 52));

            Average.Day = AllTime.Words / Math.Max(ts.Days, 1);

            AveragesEstimated = ts.Days < 365;
        }

        private Statistic GetStatistic(List<SessionModel> sessions)
        {
            var statistic = new Statistic();

            sessions = sessions.OrderBy(x => x.End).ToList();

            // Get the number of chapters, scenes and words written

            if (sessions.Count == 1)
            {
                statistic.Chapters = sessions[0].ChapterCount;

                statistic.Scenes = sessions[0].SceneCount;

                statistic.Words = sessions[0].WordCountEnd - sessions[0].WordCountStart;
            }
            else
            {
                for (int i = 0; i < sessions.Count; i += 2)
                {
                    if (i + 1 > sessions.Count - 1)
                        break;

                    var a = sessions[i];

                    var b = sessions[i + 1];

                    statistic.Chapters += a.ChapterCount - b.ChapterCount;

                    statistic.Scenes += a.SceneCount - b.SceneCount;

                    statistic.Words += (a.WordCountEnd - a.WordCountStart) + (b.WordCountEnd - b.WordCountStart);
                }
            }

            // Get the time spent writing

            var durations = new List<TimeSpan>();

            sessions.ForEach(x => durations.Add(x.End - x.Start));

            statistic.Time = new TimeSpan(durations.Sum(x => x.Ticks));

            return statistic;
        }


    }

    public class Statistic
    {
        public TimeSpan Time { get; set; }
        public int Chapters { get; set; }
        public int Scenes { get; set; }
        public int Words { get; set; }
    }

    public class WordAverage
    {
        public int AllTime { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Week { get; set; }
        public int Day { get; set; }
    }

    public class DateCutoff
    {
        public DateTime Day { get; set; }
        public DateTime Week { get; set; }
        public DateTime Month { get; set; }
        public DateTime Year { get; set; }
    }

}