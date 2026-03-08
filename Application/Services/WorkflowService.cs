using ntcc_admin_blazor.Application.DTOs;
using ntcc_admin_blazor.Domain.Entities;
using ntcc_admin_blazor.Domain.Enums;
using ntcc_admin_blazor.Services;

namespace ntcc_admin_blazor.Application.Services
{
    public class WorkflowService : IWorkflowService
    {
        private readonly SupabaseService _supabase;

        public WorkflowService(SupabaseService supabase)
        {
            _supabase = supabase;
        }

        public async Task<StageDashboardDto?> GetStageDashboardAsync(string studentId, long stageId)
        {
            var dto = new StageDashboardDto();
            
            // 1. Get Stage Info
            var stages = await _supabase.GetWhere<WorkflowStageEntity>("id", stageId);
            var stage = stages.FirstOrDefault();
            if (stage == null) return null;
            
            dto.StageId = stage.Id;
            dto.StageName = stage.StageKey;

            // 2. Get All Steps for this stage
            var allSteps = await _supabase.GetWhere<WorkflowStepEntity>("stage_id", stageId);
            
            // 3. Get Student's progress for these steps
            var studentProgress = await _supabase.GetWhere<StudentWorkflowStepEntity>("student_id", studentId);

            foreach (var step in allSteps.OrderBy(s => s.OrderIndex))
            {
                var progress = studentProgress.FirstOrDefault(p => p.StepId == step.Id);
                
                dto.Steps.Add(new WorkflowStepDto
                {
                    Id = step.Id,
                    StepKey = step.StepKey,
                    Name = step.StepKey,
                    OrderIndex = step.OrderIndex,
                    Status = progress != null ? Enum.Parse<StepStatus>(progress.Status) : StepStatus.Pending,
                    SubmittedAt = progress?.SubmittedAt
                });
            }

            // 4. Load Widgets (Mocked for now, will link to registry later)
            // In a real scenario, we'd lookup step_components and components_registry
            
            return dto;
        }

        public async Task<List<WorkflowStepDto>> GetStepsForStageAsync(long stageId)
        {
            var steps = await _supabase.GetWhere<WorkflowStepEntity>("stage_id", stageId);
            return steps.OrderBy(s => s.OrderIndex).Select(s => new WorkflowStepDto
            {
                Id = s.Id,
                StepKey = s.StepKey,
                Name = s.StepKey,
                OrderIndex = s.OrderIndex
            }).ToList();
        }

        public async Task<bool> SubmitStepAsync(string studentId, long stepId, string payloadJson)
        {
            // Update or Insert progress
            var progress = new StudentWorkflowStepEntity
            {
                StudentId = studentId,
                StepId = stepId,
                Status = StepStatus.Submitted.ToString(),
                SubmittedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _supabase.Upsert(progress);
            return true;
        }

        public async Task<bool> ApproveStepAsync(string studentId, long stepId, string feedback)
        {
            // Implementation for host/coordinator
            return await UpdateStepStatus(studentId, stepId, StepStatus.Approved);
        }

        public async Task<bool> RejectStepAsync(string studentId, long stepId, string feedback)
        {
            return await UpdateStepStatus(studentId, stepId, StepStatus.Rejected);
        }

        private async Task<bool> UpdateStepStatus(string studentId, long stepId, StepStatus status)
        {
             // Logic to find record and update status
             return true; 
        }
    }
}
