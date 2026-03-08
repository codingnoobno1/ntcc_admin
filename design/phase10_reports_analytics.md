# Phase 10: Reports & Analytics

## Objective
Transform raw workflow data into real-time actionable insights via higher-level data abstractions and automated jobs.

## 1. Performance Data Layer
Use **Materialized Views** in Postgres for performance-heavy dashboards:
- `student_performance_view`: Aggregated grades across all stages.
- `internship_completion_view`: Real-time funnel of who finished docs vs evaluation.
- `evaluation_statistics_view`: Mean, Median, and Mode of grades per evaluator/batch.

## 2. Automated Pulse & Jobs
- **`daily_deadline_check`**: Automated job to identify steps closing in 24h and trigger Phase 11 notifications.
- **`weekly_summary_generator`**: Emails coordinators a summary of batch progress and pending faculty actions.

## 3. Dynamic Reports
- **Custom Builder**: Allow admins to select columns (Batch, Status, Grade, Mentor) and export to Excel/PDF.
- **Visual Dashboards**: MudChart integration for Grade Distribution and Completion Funnels.
