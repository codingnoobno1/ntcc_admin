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

        public async Task<CompanyEntity?> GetCompanyAsync(string id)
        {
            var res = await _supabase.GetWhere<CompanyEntity>("id", id);
            return res.FirstOrDefault();
        }

        public async Task<string> AddCompanyAsync(CompanyEntity company)
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

        public async Task<List<InternshipEntity>> GetStudentInternshipsAsync(string studentId)
        {
            var internships = await _supabase.GetWhere<InternshipEntity>("student_id", studentId);
            return internships.OrderByDescending(i => i.CreatedAt).ToList();
        }

        public async Task<InternshipEntity?> GetInternshipAsync(string id)
        {
            var res = await _supabase.GetWhere<InternshipEntity>("id", id);
            return res.FirstOrDefault();
        }

        public async Task<string> ApplyForInternshipAsync(InternshipEntity internship)
        {
            var created = await _supabase.Insert(internship);
            await _activityLog.LogActivityAsync(internship.StudentId, "internship_applied", $"{{\"internship_id\":\"{created.Id}\"}}");
            return created.Id;
        }

        public async Task<bool> UpdateInternshipStatusAsync(string internshipId, string newStatus)
        {
            var internship = await GetInternshipAsync(internshipId);
            if (internship == null) return false;

            internship.Status = newStatus;
            await _supabase.Update(internship);
            
            await _activityLog.LogActivityAsync(internship.StudentId, "internship_updated", $"{{\"status\":\"{newStatus}\"}}");
            return true;
        }
    }
}
