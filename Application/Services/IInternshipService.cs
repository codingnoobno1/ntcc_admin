using ntcc_admin_blazor.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ntcc_admin_blazor.Application.Services
{
    public interface IInternshipService
    {
        // Company Registry
        Task<List<CompanyEntity>> GetAllCompaniesAsync();
        Task<CompanyEntity?> GetCompanyAsync(string id);
        Task<string> AddCompanyAsync(CompanyEntity company);
        Task<bool> UpdateCompanyAsync(CompanyEntity company);

        // Internships
        Task<List<InternshipEntity>> GetAllInternshipsAsync();
        Task<List<InternshipEntity>> GetStudentInternshipsAsync(string studentId);
        Task<InternshipEntity?> GetInternshipAsync(string id);
        Task<string> ApplyForInternshipAsync(InternshipEntity internship);
        Task<bool> UpdateInternshipStatusAsync(string internshipId, string newStatus);
    }
}
