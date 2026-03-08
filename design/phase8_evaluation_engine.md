# Phase 8: Dynamic Evaluation & Rubric Engine

## Objective
Support multi-perspective grading (Internal/External) and secure the evaluation integrity via locking mechanisms.

## 1. Multi-Evaluator Support
- **`evaluation_assignments`**: Explicitly maps evaluators to students.
  - Supports roles like `Internal Examiner`, `External Expert`, `Supervisor`.
- **Aggregate Logic**: Configurable rules to determine the final grade (e.g., "Average of All", "Highest Score", or "Supervisor Weightage").

## 2. Integrity & Locking
- **`evaluation_status`**:
  - `is_locked` (boolean): Prevents any modifications after a grade is finalized.
  - `finalized_by`, `finalized_at`.
- **Validation**: Marks cannot exceed the `max_marks` defined in the rubric component.

## 3. Components
- `RubricGrader`: Dynamically renders fields for Analysis, Presentation, Viva, etc.
- `AuditTrail`: Displays who awarded which marks if multiple evaluators are involved.
