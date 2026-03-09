using System.Linq;
using System.Threading.Tasks;
using ntcc_admin_blazor.Domain.Entities;

namespace ntcc_admin_blazor.Services
{
    public class AcademicConfig
    {
        public int OddSemStartMonth { get; set; } = 8; // Default Aug
        public int OddSemStartDay { get; set; } = 16;
        public int EvenSemStartMonth { get; set; } = 2; // Default Feb
        public int EvenSemStartDay { get; set; } = 1;
    }

    public interface IAcademicCalendarService
    {
        int CalculateCurrentSemester(int startYear, int totalYears, AcademicConfig? config = null);
        List<BatchSemesterEntity> GenerateBatchTimeline(string batchId, int startYear, int totalYears, AcademicConfig? config = null);
        (DateTime Start, DateTime End) GetSemesterDateRange(int startYear, int semesterNumber, AcademicConfig? config = null);
        Task<AcademicStageRuleEntity?> GetActiveStage(string programId, int semesterNumber);
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
            return rules.FirstOrDefault(r => r.SemesterNumber == semesterNumber);
        }
        public int CalculateCurrentSemester(int startYear, int totalYears, AcademicConfig? config = null)
        {
            config ??= new AcademicConfig();
            var now = DateTime.Now;
            int currentYear = now.Year;
            
            int yearsPassed = currentYear - startYear;
            if (yearsPassed < 0) return 0;

            // Determine if current date is in Even or Odd sem based on config
            DateTime evenStart = new DateTime(currentYear, config.EvenSemStartMonth, config.EvenSemStartDay);
            DateTime oddStart = new DateTime(currentYear, config.OddSemStartMonth, config.OddSemStartDay);

            bool isEvenSem;
            if (config.EvenSemStartMonth < config.OddSemStartMonth)
            {
                // Normal case: Feb < Aug
                isEvenSem = now >= evenStart && now < oddStart;
            }
            else
            {
                // Wrapped case (unlikely but handled)
                isEvenSem = now >= evenStart || now < oddStart;
            }
            
            int semester = (yearsPassed * 2) + (isEvenSem ? 2 : 1);
            return Math.Min(semester, totalYears * 2);
        }

        public List<BatchSemesterEntity> GenerateBatchTimeline(string batchId, int startYear, int totalYears, AcademicConfig? config = null)
        {
            config ??= new AcademicConfig();
            var timeline = new List<BatchSemesterEntity>();
            int totalSemesters = totalYears * 2;

            for (int sem = 1; sem <= totalSemesters; sem++)
            {
                var range = GetSemesterDateRange(startYear, sem, config);
                timeline.Add(new BatchSemesterEntity
                {
                    Id = Guid.NewGuid().ToString(),
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
                // Odd Sem Start
                var start = new DateTime(currentYear, config.OddSemStartMonth, config.OddSemStartDay);
                // Ends day before Even Sem Start of NEXT year if Wrapped, or SAME year
                var endYear = (config.EvenSemStartMonth < config.OddSemStartMonth) ? currentYear + 1 : currentYear;
                var end = new DateTime(endYear, config.EvenSemStartMonth, config.EvenSemStartDay).AddDays(-1);
                return (start, end);
            }
            else
            {
                // Even Sem Start
                var start = new DateTime(currentYear, config.EvenSemStartMonth, config.EvenSemStartDay);
                // Ends day before Odd Sem Start of SAME year
                var end = new DateTime(currentYear, config.OddSemStartMonth, config.OddSemStartDay).AddDays(-1);
                return (start, end);
            }
        }
    }
}
