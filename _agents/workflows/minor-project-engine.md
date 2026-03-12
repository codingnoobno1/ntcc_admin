---
description: Phase 10: Minor Project Engine Architecture
---

# Minor Project Engine Workflow

The Minor Project module provides end-to-end management of the academic project lifecycle, acting as the workflow engine connecting Stages, Submissions, Faculty, and Evaluations.

## System Components

### 1. Stage Engine (/projects/minor/stages)
Admin interface for defining rules, deadlines, and required documents.
- **UI**: Vis.js Timeline for workflow visualization, MudBlazor forms for stage editing, MudDatePicker for deadlines.
- **Workflow Example**: Title Approval -> Proposal Submission -> Mid Term -> Draft Report -> End Term -> Final Submission.

### 2. Document Segregation System (/projects/minor/documents)
Tracking and management of student submissions.
- **UI**: Radzen DataGrid for filtering/sorting student document status (Proposal, Draft, Final, IEEE Paper, Patent).
- **Features**: Side-by-side grid and PDF.js viewer.

### 3. Evaluation Criteria Configuration (/projects/minor/evaluation-rules)
Dynamic rubric builder for faculty grading.
- **UI**: MudExpansionPanel and dynamic MudBlazor forms for building categories (e.g., Mid Term, End Term, Supervisor Evaluation).
- **Features**: Define components like Analysis (10), Presentation (10), Viva (10).

### 4. Group Formation (/projects/minor/groups)
Interface for students/admins to build project teams.
- **UI**: Blazor DragDrop interface.
- **Features**: Enforce team sizes (3-4), assign leaders.

### 5. Supervisor Assignment (/projects/minor/supervisors)
Mapping grid to connect Faculty mentors to Project Groups.
- **UI**: Radzen DataGrid.
- **Features**: Bulk assignment, department filtering.

### 6. Weekly Supervisor Reports (/projects/minor/weekly-reports)
Workflow for mentors to track ongoing progress.
- **UI**: Two-panel layout (Student Info | Quill Editor for Rich Text).
- **Features**: ApexCharts for progress over time and meeting frequency.

### 7. Student Progress Analysis (/projects/minor/analysis/{studentId})
Complete 360-degree overview of a single student.
- **UI**: Student Profile, Project Progress Chart, Weekly Reports Timeline, Evaluation Marks, Document Status.
- **Features**: ApexCharts for visualization.

### 8. Evaluation Workspace (/projects/minor/evaluation)
Side-by-side grading interface for Faculty.
- **UI**: Split view with PDF.js on the left and MudBlazor scoring rubric on the right.

## Verification
1. Install and configure required external JS/CSS dependencies in `index.html` / `App.razor` (Vis.js, Radzen, PDF.js, Quill, ApexCharts, Blazor DragDrop).
2. Compile views and verify routing via the Sidebar menu.
