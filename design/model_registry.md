# Domain Models & DTO Registry

## 1. Domain Entities (Database Mapped)
### Stage & Workflow
- `StageEntity`: `id`, `name`, `program_id`, `semester`.
- `WorkflowStepEntity`: `id`, `stage_id`, `step_key`, `order_index`.
- `StepComponentEntity`: `step_id`, `component_key`.
- `StudentWorkflowStep`: `student_id`, `step_id`, `status`, `submitted_at`.

### Evaluation
- `EvaluationScheme`: `id`, `stage_id`, `name`.
- `EvaluationCategory`: `id`, `scheme_id`, `name`, `weight`.
- `EvaluationComponent`: `id`, `category_id`, `name`, `max_marks`.
- `EvaluationScore`: `student_id`, `component_id`, `marks`, `faculty_id`.

### Tracking
- `AttendanceRecord`: `student_id`, `stage_id`, `date`, `is_present`.
- `MentorMeeting`: `student_id`, `stage_id`, `meeting_date`, `notes_json`.
- `StudentActivityLog`: `student_id`, `action`, `metadata_json`, `created_at`.

## 2. Data Transfer Objects (DTOs)
- `DashboardWidgetDto`: `ComponentKey`, `Type`, `ConfigJson`, `Order`.
- `GradeSubmissionDto`: `StudentId`, `StageId`, `Scores` (Dictionary of ComponentId -> Marks), `Feedback`.
- `GroupProposalDto`: `Title`, `Members` (List), `Abstract`.
- `TimelineEventDto`: `Label`, `Timestamp`, `Status`, `IsCurrent`.

## 3. Enumerations
- `StepStatus`: `Pending`, `Submitted`, `Approved`, `Rejected`.
- `ComponentType`: `Metric`, `Visualization`, `Academic`, `Evaluation`, `Productivity`.
- `UserRole`: `Admin`, `Coordinator`, `Mentor`, `Evaluator`, `Student`.
