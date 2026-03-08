# NTCC Admin Dashboard - In-Depth Technical Analysis

## Executive Summary
The NTCC (Non-Teaching Credit Course) Admin Dashboard is a sophisticated web application designed to manage the academic lifecycle of vocational training, internships, and projects. Built on the **Blazor Web App** framework (.NET 8), it leverages **Supabase** for its backend infrastructure and **MudBlazor** for a premium, responsive UI/UX.

---

## 1. Architectural Overview
The application follows a clean, layered architecture that separates concerns between UI, business logic, and infrastructure.

### Technology Stack
- **Frontend**: Blazor Interactive Server (C# / Razor)
- **UI Components**: MudBlazor (Vibrant, glassmorphism-inspired design)
- **Backend-as-a-Service**: Supabase (Database, Auth, Storage)
- **Data Access**: Postgrest.Net (ORM-like interaction with Supabase)
- **State Management**: AuthenticationStateProvider & Dependency Injection

### Layered Structure
- **Web Layer (`Components/`)**: Contains Razor pages and shared components (Layouts, NavMenu).
- **Application Layer (`Application/Services/`)**: Implements business use cases via AppServices (e.g., `FacultyAppService`, `StudentAppService`).
- **Domain Layer (`Domain/Entities/`)**: Defines the data models and their mapping to Supabase tables.
- **Service Layer (`Services/`)**: Handles low-level infrastructure concerns like Supabase client initialization and generic CRUD operations.

---

## 2. Core Modules & Features

### A. Authentication & Authorization
Uses Supabase Auth integrated with Blazor's native `AuthenticationStateProvider`.
- **Roles**: Supports `admin`, `faculty`, and `student`.
- **Security**: Attribute-based authorization (`[Authorize]`) protects sensitive pages and API calls.
- **Identity Management**: User profiles are synchronized between Supabase Auth and the `profiles` table.

### B. Stage Configuration Engine
Perhaps the most complex part of the system, allowing administrators to define "Stages" (like Minor Project or Summer Internship).
- **Milestone Management**: Sequential deadlines with specific dates.
- **Submission Rules**: Define what file types (PDF, ZIP, LINK) are required for each milestone.
- **Evaluation Rubrics**: Multi-category grading system (e.g., Viva, Report, Demo) with custom weightage.
- **Requirement Logic**: Thresholds for supervisor meetings, attendance, or credits.

### C. Faculty Action Center & Dashboard
A dedicated space for faculty to monitor their workload.
- **KPI Tracking**: Real-time stats on assigned students and pending evaluations.
- **Activity Pulse**: A timeline of recent student submissions and grading actions.
- **Lifecycle Tracker**: A bird's eye view of student progress across all NTCC stages.

### D. Student Lifecycle Management
- **Directory**: searchable list of all students with batch/semester filtering.
- **Progress Tracking**: Detailed timeline view of a student's status in various academic stages.
- **Enrolment**: Automated workflows to enroll batches into specific NTCC modules.

### E. Evaluation & Grading System
- **Rubric-based Grading**: Faculty grade students against the specific components defined in the Stage Config.
- **Feedback Loop**: Integrated remarks and feedback for each evaluated component.
- **Score Calculation**: Automated tallying based on rubric weightage.

---

## 3. Data Model Insights
The system uses a relational schema hosted on PostgreSQL via Supabase.

| Entity Group | Key Tables | Purpose |
| :--- | :--- | :--- |
| **Organizational** | `programs`, `batches` | Academic hierarchy and student cohorts. |
| **Operational** | `projects`, `internships`, `submissions` | Core academic activities and assets. |
| **Lifecycle** | `ntcc_stages`, `student_stage_progress` | Defines and tracks the progress of NTCC modules. |
| **Evaluation** | `evaluation_categories`, `evaluation_scores` | Rubric definitions and student grades. |
| **Configuration** | `system_settings`, `stage_deadlines` | App-wide and stage-specific rules. |

---

## 4. UI/UX Design Philosophy
The dashboard utilizes **Glassmorphism** and **Premium Aesthetics**:
- **Visuals**: Blur effects (`backdrop-filter`), subtle gradients, and rounded corners (16px - 24px).
- **Interactivity**: Smooth transitions, hover effects on KPI cards, and responsive navigation drawers.
- **Typography**: Uses modern, bold fonts (Inter/Outfit) with a focus on hierarchy and readability.

---

## 5. Recent Engineering Improvements
- **Provider Centralization**: Moved MudBlazor providers to `Routes.razor` for global availability and v6+ compatibility.
- **Route Resolution**: Fixed ambiguity issues by implementing unique "Hub" routes for complex pages.
- **Data Integrity**: Replaced simulated/fake data with live Supabase service calls across all management hubs.
