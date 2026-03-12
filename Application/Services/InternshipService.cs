using ntcc_admin_blazor.Domain.Entities;
using ntcc_admin_blazor.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ntcc_admin_blazor.Application.Services
{
    public class InternshipService : IInternshipService
    {
        private readonly SupabaseService _supabase;
        private readonly IActivityLogService _activityLog;

        public InternshipService(SupabaseService supabase, IActivityLogService activityLog)
        {
            _supabase = supabase;
            _activityLog = activityLog;
        }

        public async Task<List<CompanyEntity>> GetAllCompaniesAsync()
        {
            var companies = await _supabase.GetAll<CompanyEntity>();
            return companies.OrderBy(c => c.Name).ToList();
        }

        public async Task<CompanyEntity?> GetCompanyAsync(Guid id)
        {
            var res = await _supabase.GetWhere<CompanyEntity>("id", id.ToString());
            return res.FirstOrDefault();
        }

        public async Task<Guid> AddCompanyAsync(CompanyEntity company)
        {
            var created = await _supabase.Insert(company);
            return created.Id;
        }

        public async Task<bool> UpdateCompanyAsync(CompanyEntity company)
        {
            await _supabase.Update(company);
            return true;
        }

        public async Task<List<InternshipEntity>> GetAllInternshipsAsync()
        {
            var internships = await _supabase.GetAll<InternshipEntity>();
            return internships.OrderByDescending(i => i.CreatedAt).ToList();
        }

        public async Task<List<InternshipEntity>> GetStudentInternshipsAsync(Guid studentId)
        {
            var internships = await _supabase.GetWhere<InternshipEntity>("student_id", studentId.ToString());
            return internships.OrderByDescending(i => i.CreatedAt).ToList();
        }

        public async Task<InternshipEntity?> GetInternshipAsync(Guid id)
        {
            var res = await _supabase.GetWhere<InternshipEntity>("id", id.ToString());
            return res.FirstOrDefault();
        }

        public async Task<Guid> ApplyForInternshipAsync(InternshipEntity internship)
        {
            var created = await _supabase.Insert(internship);

            await _activityLog.LogActivityAsync(
                internship.StudentId,
                "internship_applied",
                $"{{\"internship_id\":\"{created.Id}\"}}"
            );

            return created.Id;
        }

        public async Task<bool> UpdateInternshipStatusAsync(Guid internshipId, string newStatus)
        {
            var internship = await GetInternshipAsync(internshipId);
            if (internship == null) return false;

            internship.Status = newStatus;

            await _supabase.Update(internship);

            await _activityLog.LogActivityAsync(
                internship.StudentId,
                "internship_updated",
                $"{{\"status\":\"{newStatus}\"}}"
            );

            return true;
        }
    }
}