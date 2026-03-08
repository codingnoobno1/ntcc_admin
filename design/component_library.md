# Component Library: 50+ Modular Widgets

This document lists the required UI components for the NTCC Dashboard, categorized by their functional domain.

## 1. Metric widgets (12)
1. `SummaryCard`: Generic KPI card.
2. `StudentCountWidget`: Total students in active stage.
3. `DeadlineCountdown`: Timer for the next milestone.
4. `SubmissionRateGauge`: Circular progress of batch submissions.
5. `GradingProgressMetric`: % of students evaluated.
6. `InternshipCompletionMetric`: % of successful certificate uploads.
7. `AverageGradeMetric`: Real-time batch average.
8. `FacultyWorkloadCard`: Number of students assigned to active user.
9. `PendingApprovalCounter`: Alert for mentors.
10. `AuditFailureTicker`: List of recent document rejections.
11. `ActiveGroupCounter`: Number of formalized project teams.
12. `SystemHealthBadge`: API/Supabase status indicator.

## 2. Visualization Components (10)
13. `StageTimeline`: Vertical progress tracker for students.
14. `BatchAttendanceHeatmap`: Student engagement visualization.
15. `GradeDistributionChart`: Bar chart of score spreads.
16. `DeadlineBurnDown`: Trend chart for submission dates.
17. `InternshipSectorPie`: Industry distribution of internships.
18. `SubmissionTimeline`: Stream of recent student activities.
19. `LifecycleRoadmap`: Multi-semester progress map.
20. `EvaluatorConsistencyChart`: Comparison of average grades across evaluators.
21. `MilestoneFunnel`: Dropout/Delay tracking at each workflow step.
22. `ProjectCategoryCloud`: Tag cloud of project titles/themes.

## 3. Academic & Workflow widgets (15)
23. `ProjectTitleForm`: Submission form with title/abstract.
24. `DocumentUploadWidget`: Version-controlled file upload.
25. `GroupFormationWizard`: Invitation-based team builder.
26. `ProposalReviewPanel`: Faculty UI for approving titles.
27. `MentorMeetingTracker`: Interactive log of supervisor sessions.
28. `WeeklyLogEditor`: Rich text editor for student journals.
29. `AttendanceCheckIn`: QR or self-declaration presence tool.
30. `SupervisorAssignmentTool`: Drag-and-drop student-mentor mapping.
31. `RequirementChecklist`: Confirmation of prerequisites (Sem 5 passed, etc).
32. `StageSwitcher`: Navigation tool for historical data.
33. `DocumentVerifier`: Side-by-side view for auditing PDFs.
34. `GroupMemberManager`: Leader tool for adding/removing members.
35. `InternshipSlotMap`: Registry of available company seats.
36. `NOCGenerator`: Automated creation of No-Objection Certificates.
37. `PeerEvaluationTool`: Student-to-student feedback module.

## 4. Evaluation & Grading widgets (10)
38. `RubricGradingPanel`: Dynamic form generated from evaluation_schemes.
39. `MidTermScorecard`: Form for analysis/presentation/viva.
40. `FinalThesisGrader`: Comprehensive component grading.
41. `SupervisorEvaluationPanel`: Mentor-specific grading.
42. `GradeCalculator`: Real-time tallying of rubric items.
43. `FeedbackThread`: Contextual comments on grading items.
44. `ExternalExpertPanel`: Minimum UI for guest evaluators.
45. `ScoreOverrideModule`: Coordinator tool for grade corrections.
46. `PlagiarismReportWidget`: Display of similarity scores.
47. `EvaluationQueue`: Priority list of students awaiting grading.

## 5. Administrative & Productivity (8)
48. `QuickEmailSender`: Broadcast alerts to batch/stage groups.
49. `DeadlineManager`: Global UI for shifting milestone dates.
50. `BatchImporter`: CSV/Excel student onboarding.
51. `RoleMatrixEditor`: Toggle permissions for roles.
52. `SystemConfigurationPanel`: Global settings (max group size, etc).
53. `AuditLogViewer`: Searchable history of system actions.
54. `PDFReportGenerator`: Bulk export of grade sheets.
55. `NotificationCenter`: Real-time alert tray.
