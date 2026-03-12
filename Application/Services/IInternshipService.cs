using ntcc_admin_blazor.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ntcc_admin_blazor.Application.Services
{
    public interface IInternshipService
    {
        // Company Registry
        Task<List<CompanyEntity>> GetAllCompaniesAsync();
        Task<CompanyEntity?> GetCompanyAsync(Guid id);
        Task<Guid> AddCompanyAsync(CompanyEntity company);
        Task<bool> UpdateCompanyAsync(CompanyEntity company);

        // Internships
        Task<List<InternshipEntity>> GetAllInternshipsAsync();
        Task<List<InternshipEntity>> GetStudentInternshipsAsync(Guid studentId);
        Task<InternshipEntity?> GetInternshipAsync(Guid id);
        Task<Guid> ApplyForInternshipAsync(InternshipEntity internship);
        Task<bool> UpdateInternshipStatusAsync(Guid internshipId, string newStatus);
    }
}
