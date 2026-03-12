using ntcc_admin_blazor.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ntcc_admin_blazor.Application.Services
{
    public interface IProjectFormationService
    {
        Task<ProjectGroupEntity?> GetGroupAsync(Guid groupId);
        Task<List<ProjectGroupEntity>> GetGroupsForStageAsync(Guid stageId);
        Task<ProjectGroupEntity?> GetStudentActiveGroupAsync(Guid studentId, Guid stageId);
        
        Task<Guid> CreateGroupAsync(ProjectGroupEntity group, Guid leaderStudentId);
        Task<bool> InviteMemberAsync(Guid groupId, Guid studentId);
        Task<bool> RespondToInviteAsync(Guid groupId, Guid studentId, bool accept);
        
        Task<bool> SubmitProposalAsync(Guid groupId);
        Task<bool> VetProposalAsync(Guid groupId, Guid facultyId, bool isApproved, string remarks);
    }
}
