# Project Blueprint: 60+ Essential Files

This list outlines the directory structure and the core files required to implement the Stage Engine and Dynamic Dashboard.

## 1. Domain & Models (12 files)
- `Domain/Entities/WorkflowEntities.cs`: (Stages, Steps, Progress)
- `Domain/Entities/EvaluationEntities.cs`: (Schemes, Categories, Scores)
- `Domain/Entities/AcademicTrackingEntities.cs`: (Attendance, Meetings, Logs)
- `Domain/Entities/RBACEntities.cs`: (Roles, Permissions, Overrides)
- `Application/DTOs/WorkflowDtos.cs`
- `Application/DTOs/EvaluationDtos.cs`
- `Application/DTOs/DashboardDtos.cs`
- `Domain/Enums/StageEnums.cs`
- `Domain/Enums/RBACEnums.cs`
- `Domain/Constants/ComponentKeys.cs`
- `Domain/Constants/PermissionKeys.cs`
- `Domain/Common/BaseEntity.cs`

## 2. Application Services (10 files)
- `Application/Services/WorkflowService.cs`
- `Application/Services/PermissionService.cs`
- `Application/Services/EvaluationService.cs`
- `Application/Services/DashboardService.cs`
- `Application/Services/GroupFormationService.cs`
- `Application/Services/InternshipService.cs`
- `Application/Services/AttendanceService.cs`
- `Application/Services/NotificationService.cs`
- `Application/Services/StudentAppService.cs`
- `Application/Services/AuditService.cs`

## 3. Components - Dashboard Engine (5 files)
- `Components/Dashboard/WidgetContainer.razor`
- `Components/Dashboard/WidgetContainer.razor.cs`
- `Components/Dashboard/DashboardOrchestrator.razor`
- `Components/Dashboard/DynamicLayoutManager.razor`
- `Components/Dashboard/SkeletonLoader.razor`

## 4. Components - Metric Widgets (12 files)
- `Components/Widgets/Metrics/MetricCard.razor`
- `Components/Widgets/Metrics/DeadlineCountdown.razor`
- `Components/Widgets/Metrics/GradingCompletionGauge.razor`
- `Components/Widgets/Metrics/ActiveGroupCounter.razor`
- ... (Additional 8 Metric files)

## 5. Components - Visualizations (10 files)
- `Components/Widgets/Visuals/StageTimeline.razor`
- `Components/Widgets/Visuals/AttendanceHeatmap.razor`
- `Components/Widgets/Visuals/GradeDistributionChart.razor`
- `Components/Widgets/Visuals/SubmissionPulse.razor`
- ... (Additional 6 Visualization files)

## 6. Components - Academic & Evaluation (15 files)
- `Components/Widgets/Academic/ProjectTitleForm.razor`
- `Components/Widgets/Academic/DocumentUploader.razor`
- `Components/Widgets/Academic/GroupFormationWizard.razor`
- `Components/Widgets/Academic/MeetingTracker.razor`
- `Components/Widgets/Evaluation/RubricGrader.razor`
- `Components/Widgets/Evaluation/FeedbackHub.razor`
- ... (Additional 9 Academic/Eval files)

## 7. Pages & Layouts (8 files)
- `Components/Pages/Dashboard.razor`: (The core dynamic page)
- `Components/Pages/Admin/StageBuilder.razor`: (Configuration UI)
- `Components/Pages/Admin/PermissionMatrix.razor`
- `Components/Pages/Admin/StudentDirectory.razor`
- `Components/Pages/Faculty/EvaluationQueue.razor`
- `Components/Layout/MainLayout.razor`
- `Components/Layout/NavMenu.razor`
- `Components/Pages/Auth/Login.razor`
