using System.Linq;
using System.Threading.Tasks;
using ntcc_admin_blazor.Domain.Entities;

namespace ntcc_admin_blazor.Services
{
    public class AcademicConfig
    {
        public int OddSemStartMonth { get; set; } = 8;
        public int OddSemStartDay { get; set; } = 16;
        public int OddSemEndMonth { get; set; } = 1;
        public int OddSemEndDay { get; set; } = 31;

        public int EvenSemStartMonth { get; set; } = 2;
        public int EvenSemStartDay { get; set; } = 1;
        public int EvenSemEndMonth { get; set; } = 6;
        public int EvenSemEndDay { get; set; } = 30;
    }

    public interface IAcademicCalendarService
    {
        int CalculateCurrentSemester(int startYear, int totalYears, AcademicConfig? config = null);
        List<BatchSemesterEntity> GenerateBatchTimeline(Guid batchId, int startYear, int totalYears, AcademicConfig? config = null);
        (DateTime Start, DateTime End) GetSemesterDateRange(int startYear, int semesterNumber, AcademicConfig? config = null);
        Task<AcademicStageRuleEntity?> GetActiveStage(string programId, int semesterNumber);
        IEnumerable<(DateTime Start, DateTime End, string Type, int? SemNum)> GetFullTimeline(int startYear, int totalYears, AcademicConfig? config = null);
    }

    public class AcademicCalendarService : IAcademicCalendarService
    {
        private readonly SupabaseService _supabase;

        public AcademicCalendarService(SupabaseService supabase)
        {
            _supabase = supabase;
        }

        public async Task<AcademicStageRuleEntity?> GetActiveStage(string programId, int semesterNumber)
        {
            var rules = await _supabase.GetWhere<AcademicStageRuleEntity>("program_id", programId);
            return rules.FirstOrDefault(r => r.SemesterNumber == semesterNumber && r.Status == "active");
        }
        public int CalculateCurrentSemester(int startYear, int totalYears, AcademicConfig? config = null)
        {
            var now = DateTime.Now;
            int yearsDiff = now.Year - startYear;
            
            // Simple Logic:
            // July-Dec (Month > 6) -> Odd Semester: (yearsDiff * 2) + 1
            // Jan-June (Month <= 6) -> Even Semester: (yearsDiff * 2)
            
            int sem = (yearsDiff * 2);
            if (now.Month > 6)
            {
                sem += 1;
            }

            // Clamping
            return Math.Max(1, Math.Min(sem, totalYears * 2));
        }

        public List<BatchSemesterEntity> GenerateBatchTimeline(Guid batchId, int startYear, int totalYears, AcademicConfig? config = null)
        {
            config ??= new AcademicConfig();
            var timeline = new List<BatchSemesterEntity>();
            int totalSemesters = totalYears * 2;

            for (int sem = 1; sem <= totalSemesters; sem++)
            {
                var range = GetSemesterDateRange(startYear, sem, config);
                timeline.Add(new BatchSemesterEntity
                {
                    Id = Guid.NewGuid(),
                    BatchId = batchId,
                    SemesterNumber = sem,
                    StartDate = range.Start,
                    EndDate = range.End,
                    IsActive = false
                });
            }

            return timeline;
        }

        public (DateTime Start, DateTime End) GetSemesterDateRange(int startYear, int semesterNumber, AcademicConfig? config = null)
        {
            config ??= new AcademicConfig();
            int yearOffset = (semesterNumber - 1) / 2;
            int currentYear = startYear + yearOffset;
            bool isOdd = semesterNumber % 2 != 0;

            if (isOdd)
            {
                var start = new DateTime(currentYear, config.OddSemStartMonth, config.OddSemStartDay);
                var endYear = (config.OddSemEndMonth < config.OddSemStartMonth) ? currentYear + 1 : currentYear;
                var end = new DateTime(endYear, config.OddSemEndMonth, config.OddSemEndDay);
                return (start, end);
            }
            else
            {
                var start = new DateTime(currentYear, config.EvenSemStartMonth, config.EvenSemStartDay);
                var endYear = (config.EvenSemEndMonth < config.EvenSemStartMonth) ? currentYear + 1 : currentYear;
                var end = new DateTime(endYear, config.EvenSemEndMonth, config.EvenSemEndDay);
                return (start, end);
            }
        }

        public IEnumerable<(DateTime Start, DateTime End, string Type, int? SemNum)> GetFullTimeline(int startYear, int totalYears, AcademicConfig? config = null)
        {
            config ??= new AcademicConfig();
            var timeline = new List<(DateTime Start, DateTime End, string Type, int? SemNum)>();
            int totalSems = totalYears * 2;

            for (int i = 1; i <= totalSems; i++)
            {
                var semRange = GetSemesterDateRange(startYear, i, config);
                
                // Add break before if not the first and there's a gap
                if (timeline.Any())
                {
                    var lastEnd = timeline.Last().End;
                    if (semRange.Start > lastEnd.AddDays(1))
                    {
                        timeline.Add((lastEnd.AddDays(1), semRange.Start.AddDays(-1), "Break", null));
                    }
                }

                timeline.Add((semRange.Start, semRange.End, "Semester", i));
            }

            return timeline;
        }
    }
}
