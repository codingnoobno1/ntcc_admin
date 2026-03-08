using ntcc_admin_blazor.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ntcc_admin_blazor.Application.Services
{
    public interface IProjectFormationService
    {
        Task<ProjectGroupEntity?> GetGroupAsync(string groupId);
        Task<List<ProjectGroupEntity>> GetGroupsForStageAsync(string stageId);
        Task<ProjectGroupEntity?> GetStudentActiveGroupAsync(string studentId, string stageId);
        
        Task<string> CreateGroupAsync(ProjectGroupEntity group, string leaderStudentId);
        Task<bool> InviteMemberAsync(string groupId, string studentId);
        Task<bool> RespondToInviteAsync(string groupId, string studentId, bool accept);
        
        Task<bool> SubmitProposalAsync(string groupId);
        Task<bool> VetProposalAsync(string groupId, string facultyId, bool isApproved, string remarks);
    }
}
