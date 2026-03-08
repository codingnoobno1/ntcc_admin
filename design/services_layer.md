# Application Services & API Layer

## 1. Core Services
### `WorkflowService.cs`
- `GetActiveStageAsync(Guid studentId)`
- `GetCurrentStepAsync(Guid studentId, long stageId)`
- `SubmitStepAsync(Guid studentId, long stepId, object payload)`
- `ReviewStepAsync(long workflowStepId, StepStatus status, string feedback)`
- `GetWorkflowHistory(Guid studentId, long stageId)`

### `PermissionService.cs`
- `HasRbacPermission(string key)`
- `HasWorkflowAction(long stageId, string action)`
- `GetUserRolesAsync(Guid userId)`
- `ClearPermissionCache(Guid userId)`

### `EvaluationService.cs`
- `LoadRubricAsync(long stageId)`
- `SubmitGradeAsync(GradeSubmissionDto grade)`
- `CalculateFinalScore(long studentId, long stageId)`
- `LockEvaluation(long evaluationId)`

### `DashboardService.cs`
- `GetDashboardWidgetsAsync(long stageId, string role)`
- `GetStudentTimelineAsync(Guid studentId, long stageId)`
- `GetFacultyPulseAsync(long programId)`

## 2. specialized Domain Services
- **`GroupService.cs`**: Invitations, proposal vetting, membership constraints.
- **`InternshipService.cs`**: Company verification, document auditing, certificate generation.
- **`AttendanceService.cs`**: Log capturing and threshold/eligibility checks.
- **`NotificationService.cs`**: Real-time broadcasts and persistent alert storage.

## 3. Infrastructure Services
- **`SupabaseService.cs`**: Raw CRUD, Realtime subscription management, Storage folder handling.
- **`AuditService.cs`**: Non-blocking log persistence.
- **`ExportService.cs`**: PDF/Excel generation using local templates.
