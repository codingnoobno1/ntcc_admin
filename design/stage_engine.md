# Stage Engine: Technical Architecture

## 1. Core Concept
The Stage Engine is a dynamic workflow orchestrator that transforms a static database configuration into a multi-step academic pipeline. It manages the lifecycle of major academic activities (Minor Project, Internship, etc.) by linking workflow steps to specific UI components and evaluation rubrics.

## 2. Database Schema
### `workflow_stages`
| Column | Type | Description |
| :--- | :--- | :--- |
| id | BIGINT | Primary Key |
| stage_key | TEXT | Unique key (e.g., `minor_project_2027`) |
| name | TEXT | Display name |
| program_id | BIGINT | Link to Programs |
| semester | INT | target semester |
| is_active | BOOLEAN | Toggle for registration |

### `workflow_steps`
| Column | Type | Description |
| :--- | :--- | :--- |
| id | BIGINT | Primary Key |
| stage_id | BIGINT | Parent Stage |
| step_key | TEXT | Unique within stage (e.g., `title_approval`) |
| name | TEXT | Display name |
| order_index | INT | Sequence in pipeline |
| is_blocking | BOOLEAN | If true, student can't proceed until approved |

### `stage_deadlines`
| Column | Type | Description |
| :--- | :--- | :--- |
| step_id | BIGINT | Link to Step |
| batch_id | BIGINT | specific batch context |
| deadline | TIMESTAMP | The cutoff date/time |

## 3. Workflow Logic
### State Machine
The engine tracks `student_workflow_steps` status:
- `PENDING`: Initial state.
- `SUBMITTED`: Student has uploaded required assets.
- `UNDER_REVIEW`: Locked for mentor/evaluator action.
- `REJECTED`: Returned for revisions.
- `APPROVED`: Milestone cleared.

### Dynamic Pipeline Loading
```csharp
var steps = await WorkflowService.GetStepsForStage(stageId);
var currentStep = steps.FirstOrDefault(s => s.Status != APPROVED);
var components = await ComponentService.GetComponentsForStep(currentStep.Id, userRole);
```

## 4. Academic SOP Mapping
Using the Minor Project SOP as an example:
1. **Step 1: Title Approval** (Component: `ProjectTitleForm`)
2. **Step 2: Proposal Submission** (Component: `DocumentUploadWidget`)
3. **Step 3: Mid Term Evaluation** (Component: `MidTermGradingPanel`)
4. **Step 4: Draft Report** (Component: `DocumentUploadWidget`)
5. **Step 5: End Term Evaluation** (Component: `FinalGradingPanel`)
6. **Step 6: Final Report submission** (Component: `DocumentUploadWidget`)
