using ntcc_admin_blazor.Domain.Entities;
using ntcc_admin_blazor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ntcc_admin_blazor.Application.Services
{
    public class ProjectFormationService : IProjectFormationService
    {
        private readonly SupabaseService _supabase;

        public ProjectFormationService(SupabaseService supabase)
        {
            _supabase = supabase;
        }

        public async Task<ProjectGroupEntity?> GetGroupAsync(Guid groupId)
        {
            var groups = await _supabase.GetWhere<ProjectGroupEntity>("id", groupId);
            return groups.FirstOrDefault();
        }

        public async Task<List<ProjectGroupEntity>> GetGroupsForStageAsync(Guid stageId)
        {
            return await _supabase.GetWhere<ProjectGroupEntity>("stage_id", stageId);
        }

        public async Task<ProjectGroupEntity?> GetStudentActiveGroupAsync(Guid studentId, Guid stageId)
        {
            var memberships = await _supabase.GetWhere<GroupMemberEntity>("student_id", studentId);
            var activeMember = memberships.FirstOrDefault(m => m.Status == "accepted" || m.Role == "leader");
            if (activeMember == null) return null;

            var group = await GetGroupAsync(activeMember.GroupId);
            return (group != null && group.StageId == stageId) ? group : null;
        }

        public async Task<Guid> CreateGroupAsync(ProjectGroupEntity group, Guid leaderStudentId)
        {
            // 1. Conflict Check: Is leader already in a group for this stage?
            var existingGroup = await GetStudentActiveGroupAsync(leaderStudentId, group.StageId);
            if (existingGroup != null)
                throw new InvalidOperationException("Student is already conceptually part of a team in this stage.");

            // 2. Create Group
            var createdGroup = await _supabase.Insert(group);

            // 3. Auto-assign the creator as the 'leader'
            var member = new GroupMemberEntity
            {
                GroupId = createdGroup.Id,
                StudentId = leaderStudentId,
                Role = "leader",
                Status = "accepted",
                JoinedAt = DateTime.UtcNow
            };
            await _supabase.Insert(member);

            return createdGroup.Id;
        }

        public async Task<bool> InviteMemberAsync(Guid groupId, Guid studentId)
        {
            var existingMembership = await _supabase.GetWhere<GroupMemberEntity>("student_id", studentId);
            if (existingMembership.Any(m => m.GroupId == groupId))
                throw new InvalidOperationException("Student is already invited or in this group.");
            
            // Should also check if they are in another active group for the SAME stage, 
            // but for simplicity, we do the hard constraint when they *accept* the invite.

            var invite = new GroupMemberEntity
            {
                GroupId = groupId,
                StudentId = studentId,
                Role = "member",
                Status = "invited"
            };

            await _supabase.Insert(invite);
            return true;
        }

        public async Task<bool> RespondToInviteAsync(Guid groupId, Guid studentId, bool accept)
        {
            var memberships = await _supabase.GetWhere<GroupMemberEntity>("student_id", studentId);
            var invite = memberships.FirstOrDefault(m => m.GroupId == groupId && m.Status == "invited");
            if (invite == null) return false;

            if (accept)
            {
                // Conflict Check: Are they already in another group?
                var group = await GetGroupAsync(groupId);
                if (group == null) return false;

                var existingActive = await GetStudentActiveGroupAsync(studentId, group.StageId);
                if (existingActive != null)
                    throw new InvalidOperationException("You are already active in another group for this stage.");

                invite.Status = "accepted";
                invite.JoinedAt = DateTime.UtcNow;
            }
            else
            {
                invite.Status = "rejected";
            }

            await _supabase.Update(invite);
            return true;
        }

        public async Task<bool> SubmitProposalAsync(Guid groupId)
        {
            var group = await GetGroupAsync(groupId);
            if (group == null) return false;

            if (group.Status != "draft")
                throw new InvalidOperationException("Only draft proposals can be submitted.");

            group.Status = "pending";
            group.UpdatedAt = DateTime.UtcNow;
            await _supabase.Update(group);
            return true;
        }

        public async Task<bool> VetProposalAsync(Guid groupId, Guid facultyId, bool isApproved, string remarks)
        {
            var group = await GetGroupAsync(groupId);
            if (group == null) return false;

            // In a real app, verify facultyId is the assigned mentor or coordinator.
            group.Status = isApproved ? "approved" : "rejected";
            group.UpdatedAt = DateTime.UtcNow;
            await _supabase.Update(group);
            return true;
        }
    }
}
