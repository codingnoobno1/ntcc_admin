# Phase 3: Stage Configuration & Workflow Engine

## Objective
Implement a fully configurable stage engine that supports sequential milestones, branching logic, and granular status tracking.

## 1. Core Models
- **`workflow_stages`**: Top-level activity (e.g., "Semester 6 Minor Project").
- **`workflow_steps`**: Milestones within a stage.
- **`stage_deadlines`**: Date-based targets for each step.

## 2. Advanced Workflow Logic
### Branching & Transitions
- **`workflow_transitions`**: Defines logic for moving between steps.
  - `from_step`, `to_step`, `condition` (e.g., `approved` -> Next Step, `rejected` -> Revision Step).
  - Enables skipping steps (e.g., if "Internship Approved", skip "Proposal").

### Granular Tracking
- **`student_workflow_steps`**: Tracks every student's position in the pipeline.
  - `student_id`, `step_id`, `status` (`pending`, `submitted`, `approved`, `rejected`).

## 3. Administrative Interface
- **Workflow Designer**: A visual or matrix-based tool for hosts to define steps and transitions.
- **Template Library**: Ability to clone and version entire stage workflows.
