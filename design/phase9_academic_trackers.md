# Phase 9: Academic Activity Trackers

## Objective
Capture day-to-day engagement and calculate computed metrics for eligibility and analytics.

## 1. Data Capture
- **`attendance_records`**: Captures daily/meeting presence.
- **`mentor_meeting_tracker`**: Captures interaction qualitative notes and minutes.

## 2. Analytics & Eligibility
- **`attendance_summary`**: Materialized or computed view for rapid checks.
  - `student_id`, `attendance_percent`.
- **Eligibility Rules**:
  - Logic engine: IF `attendance_percent` < 75%, THEN block `report_submission` step.
- **Alert Generator**: Identify students at risk based on participation drop-offs.

## 3. UI
- `EngagementHeatmap`: Faculty view to see which students are ghosting meetings.
- `MeetingLogBook`: Chronological display of supervisor-student interactions.
