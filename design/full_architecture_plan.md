# NTCC Academic Workflow: 12-Phase Advanced Architecture

This document serves as the master blueprint for the NTCC Management System, incorporating enterprise-grade RBAC, branching workflows, and dynamic configuration.

---

## Architecture Blueprint (12 Phases)

### [Phase 1: Core RBAC & Action Security](file:///f:/projects/QUIZ/ntcc_admin_blazor/design/phase1_rbac_foundation.md)
Goes beyond UI visibility to contextual **Action Permissions** (`workflow_permissions`). Caches permissions for $O(1)$ lookups.

### [Phase 2: Master Data & Temporal Context](file:///f:/projects/QUIZ/ntcc_admin_blazor/design/phase2_master_data.md)
Introduces `academic_terms` and formal `student_enrollments` to track progression across semesters.

### [Phase 3: Stage Configuration & Workflow Engine](file:///f:/projects/QUIZ/ntcc_admin_blazor/design/phase3_workflow_engine.md)
Supports **non-linear branching** via `workflow_transitions`. Tracks granular student status per step.

### [Phase 4: Dynamic Component Registry](file:///f:/projects/QUIZ/ntcc_admin_blazor/design/phase4_component_registry.md)
A versioned plugin architecture. Components (v1/v2) are loaded with JSON `metadata` for maximum reusability.

### [Phase 5: Student Lifecycle & Progress Tracking](file:///f:/projects/QUIZ/ntcc_admin_blazor/design/phase5_progress_tracking.md)
Driven by a rich `student_activity_log`. Renders a deep-dive event timeline of a student's entire academic history.

### [Phase 6: Project Formation & Group Engine](file:///f:/projects/QUIZ/ntcc_admin_blazor/design/phase6_group_engine.md)
Handles team collaboration with conflict resolution (unique group constraints) and tiered approval status.

### [Phase 7: Internship Lifecycle Module](file:///f:/projects/QUIZ/ntcc_admin_blazor/design/phase7_internship_module.md)
Includes a company/supervisor registry and document verification workflows for external activities.

### [Phase 8: Dynamic Evaluation & Rubric Engine](file:///f:/projects/QUIZ/ntcc_admin_blazor/design/phase8_evaluation_engine.md)
Supports multi-perspective grading (Internal/External evaluators) with secure evaluation locking.

### [Phase 9: Academic Activity Trackers](file:///f:/projects/QUIZ/ntcc_admin_blazor/design/phase9_academic_trackers.md)
Captures engagement data to drive automated eligibility checks (e.g., 75% attendance rule).

### [Phase 10: Reports & Analytics](file:///f:/projects/QUIZ/ntcc_admin_blazor/design/phase10_reports_analytics.md)
Uses materialized views for performance and automated jobs for daily deadline checks and summaries.

### [Phase 11: Notification & Messaging Engine](file:///f:/projects/QUIZ/ntcc_admin_blazor/design/phase11_notification_engine.md)
Real-time alerts via Supabase for submissions, evaluations, and approaching deadlines.

### [Phase 12: Global Configuration Engine](file:///f:/projects/QUIZ/ntcc_admin_blazor/design/phase12_configuration_engine.md)
Centralized `system_settings` to control engine parameters like group sizes and grading thresholds.

---

## Detailed Specifications
For granular technical details, refer to:
- [Stage Engine Core](file:///f:/projects/QUIZ/ntcc_admin_blazor/design/stage_engine.md)
- [Component & Widget Library (50+)](file:///f:/projects/QUIZ/ntcc_admin_blazor/design/component_library.md)
- [Services & API Layer](file:///f:/projects/QUIZ/ntcc_admin_blazor/design/services_layer.md)
- [Domain Models & DTO Registry](file:///f:/projects/QUIZ/ntcc_admin_blazor/design/model_registry.md)
- [Full Project File Blueprint (60+)](file:///f:/projects/QUIZ/ntcc_admin_blazor/design/file_structure_blueprint.md)
