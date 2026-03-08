# Phase 5: Student Lifecycle & Progress Tracking

## Objective
Visualize the complete academic journey of students through a rich, event-driven historical timeline.

## 1. Event Log Engine
- **`student_activity_log`**: Captures every granular action as a temporal event.
  - `student_id`, `action` (e.g., `proposal_upload`), `metadata` (JSON), `timestamp`.
  - Serves as the source of truth for the Activity Pulse and Timeline.

## 2. Lifecycle Visualization
- **Historical Timeline**: A drill-down view showing specific events within each stage.
  - ✔ Title Approved (Feb 12)
  - ✔ Proposal Submitted (Feb 15)
  - ⏳ Midterm Evaluation Pending
- **Status Rollup**: Color-coded indicators based on `student_workflow_steps` progress.

## 3. Phase Transitions
Automated triggers to mark a stage as `Completed` in the lifecycle when the final step in the workflow is verified.
- powers the "Lifecycle Gap" detection for prerequisites.
