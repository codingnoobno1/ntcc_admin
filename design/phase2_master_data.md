# Phase 2: Master Data & Batch Segregation

## Objective
Enable coordinators to manage large cohorts of students by organizing them into logical academic and temporal structures.

## 1. Data Hierarchy
- **`programs`**: Degree paths (B.Tech CSE, MCA).
- **`batches`**: Cohorts identified by year ranges (e.g., 2023-2027).
- **`academic_terms`**: Temporal context (e.g., Fall 2025).
- **`semesters`**: 1 through 8.

## 2. Dynamic Enrollment
- **`student_enrollments`**: Tracks a student's semester progression within a specific term.
  - `student_id`, `term_id`, `semester`.
- **`student_stage_assignment`**: Links students to specific active stages, acknowledging that stages can evolve or repeat.
  - `student_id`, `stage_id`, `assigned_at`.

## 3. Requirements
- **Batch Isolation**: Data views (Students, Projects) must filter by Batch, Semester, and Term.
- **Automated Promotion**: Tooling to bulk-promote students and trigger new Stage Assignments.
- **Import/Export**: CSV batch upload for initial student registration and enrollment.

## 4. UI/UX
- A persistent "Academic Context" selector in the header (Batch + Term).
- Statistical widgets showing distribution across semesters.
