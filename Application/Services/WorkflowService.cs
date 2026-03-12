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

        public async Task<StageDashboardDto?> GetStageDashboardAsync(string studentId, string programId, int semesterNumber)
        {
            var dto = new StageDashboardDto();
            
            // 1. Resolve Stage Rule
            var rules = await _supabase.GetWhere<AcademicStageRuleEntity>("program_id", programId);
            var rule = rules.FirstOrDefault(r => r.SemesterNumber == semesterNumber);
            if (rule == null) return null;

            // 2. Get Stage Type & Requirements
            var stageTypes = await _supabase.GetWhere<StageTypeEntity>("id", rule.StageTypeId);
            var stageType = stageTypes.FirstOrDefault();
            if (stageType == null) return null;

            dto.StageId = stageType.Id;
            dto.StageName = stageType.Name;

            var requirements = await _supabase.GetWhere<StageRequirementEntity>("stage_type_id", stageType.Id);
            var req = requirements.FirstOrDefault();
            if (req != null)
            {
                dto.RequiredMeetings = req.RequiredMeetings;
                dto.RequiredReports = req.RequiredReports;
            }

            // 3. Get Meetings count
            var meetings = await _supabase.GetWhere<SupervisorMeetingEntity>("student_id", studentId);
            dto.LoggedMeetings = meetings.Count;

            // 4. Get Submissions count
            var submissions = await _supabase.GetWhere<WorkflowSubmissionEntity>("student_id", studentId);
            dto.SubmittedReports = submissions.Count;

            // 5. Get Workflow Steps
            var workflowStages = await _supabase.GetWhere<WorkflowStageEntity>("stage_type_id", stageType.Id);
            var stage = workflowStages.FirstOrDefault();
            if (stage != null)
            {
                var steps = await _supabase.GetWhere<WorkflowStepEntity>("workflow_stage_id", stage.Id);
                var studentProgress = await _supabase.GetWhere<StudentWorkflowStepEntity>("student_id", studentId);

                foreach (var step in steps.OrderBy(s => s.OrderIndex))
                {
                    var progress = studentProgress.FirstOrDefault(p => p.WorkflowStepId == step.Id);
                    dto.Steps.Add(new WorkflowStepDto
                    {
                        Id = step.Id,
                        StepKey = step.StepKey,
                        Name = step.StepName,
                        OrderIndex = step.OrderIndex,
                        Status = progress != null ? Enum.Parse<StepStatus>(progress.Status) : StepStatus.Pending,
                        SubmittedAt = progress?.SubmittedAt,
                        RequiresSubmission = step.RequiresSubmission,
                        RequiresMeeting = step.RequiresMeeting
                    });
                }
            }

            return dto;
        }

        public async Task<List<SupervisorMeetingDto>> GetStudentMeetingsAsync(string studentId)
        {
            var entities = await _supabase.GetWhere<SupervisorMeetingEntity>("student_id", studentId);
            var reports = await _supabase.GetAll<MeetingReportEntity>(); // Optimization: filter properly in real app

            return entities.Select(e => new SupervisorMeetingDto
            {
                Id = e.Id,
                StudentId = e.StudentId,
                FacultyId = e.FacultyId,
                MeetingDate = e.MeetingDate,
                Summary = e.Summary ?? string.Empty,
                ProgressScore = e.ProgressScore,
                ReportUrl = reports.FirstOrDefault(r => r.MeetingId == e.Id)?.ReportUrl ?? string.Empty
            }).OrderByDescending(m => m.MeetingDate).ToList();
        }

        public async Task<bool> LogMeetingAsync(SupervisorMeetingDto meeting)
        {
            var entity = new SupervisorMeetingEntity
            {
                StudentId = meeting.StudentId,
                FacultyId = meeting.FacultyId,
                MeetingDate = meeting.MeetingDate ?? DateTime.Now,
                Summary = meeting.Summary,
                ProgressScore = meeting.ProgressScore
            };
            await _supabase.Insert(entity);

            if (!string.IsNullOrEmpty(meeting.ReportUrl))
            {
                await _supabase.Insert(new MeetingReportEntity
                {
                    MeetingId = entity.Id,
                    ReportUrl = meeting.ReportUrl
                });
            }
            return true;
        }

        public async Task<EvaluationRubricDto?> GetEvaluationRubricAsync(string stageTypeId, string examType)
        {
            var rubrics = await _supabase.GetWhere<EvaluationRubricEntity>("stage_type_id", stageTypeId);
            var rubric = rubrics.FirstOrDefault(r => r.ExamType == examType);
            if (rubric == null) return null;

            var components = await _supabase.GetWhere<RubricComponentEntity>("rubric_id", rubric.Id);

            return new EvaluationRubricDto
            {
                Id = rubric.Id,
                ExamType = rubric.ExamType,
                TotalMarks = rubric.TotalMarks,
                Components = components.Select(c => new RubricComponentDto
                {
                    Id = c.Id,
                    ComponentName = c.ComponentName,
                    MaxMarks = c.MaxMarks
                }).ToList()
            };
        }

        public async Task<bool> SubmitEvaluationAsync(string studentId, string rubricId, List<RubricComponentDto> components, string evaluatorId)
        {
            foreach (var comp in components)
            {
                var eval = new StudentEvaluationEntity
                {
                    StudentId = studentId,
                    RubricComponentId = comp.Id,
                    EvaluatorId = evaluatorId,
                    Marks = comp.MarksObtained,
                    Remarks = comp.Remarks
                };
                await _supabase.Insert(eval);
            }
            return true;
        }

        public async Task<bool> SubmitStepArtefactAsync(string studentId, string stepId, string fileUrl, string gdriveLink)
        {
            var submission = new WorkflowSubmissionEntity
            {
                StudentId = studentId,
                WorkflowStepId = stepId,
                FileUrl = fileUrl,
                GDriveLink = gdriveLink,
                SubmittedAt = DateTime.UtcNow
            };
            await _supabase.Insert(submission);
            
            return await UpdateStepStatusAsync(studentId, stepId, StepStatus.Submitted);
        }

        public async Task<bool> UpdateStepStatusAsync(string studentId, string stepId, StepStatus status)
        {
            var progress = new StudentWorkflowStepEntity
            {
                StudentId = studentId,
                WorkflowStepId = stepId,
                Status = status.ToString(),
                SubmittedAt = status == StepStatus.Submitted ? DateTime.UtcNow : null,
                UpdatedAt = DateTime.UtcNow
            };
            await _supabase.Upsert(progress);
            return true;
        }
    }
}
