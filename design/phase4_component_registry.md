# Phase 4: Dynamic Component Registry

## Objective
Create a flexible, versioned UI system where configurable widgets are loaded dynamically based on stage, step, and role.

## 1. The Versioned Registry
- **`components_registry`**:
  - `component_key`
  - `version` (e.g., `rubric_evaluation v2`)
  - Ensures historical workflows remain stable even as new component versions are released.

## 2. Metadata & Configuration
- **`component_config`**: Stores widget-specific parameters in JSON.
  - Example: `{ "max_score": 100, "allow_comments": true, "show_history": false }`.
  - Allows the same component (e.g., `document_uploader`) to behave differently in various steps.

## 3. Dynamic Rendering Orchestrator
The `WidgetContainer.razor` logic:
1. Identify Student's current `workflow_step`.
2. Query `step_components` and `component_config`.
3. Filter by `role_component_permissions`.
4. Render using `<DynamicComponent Type="..." Parameters="..." />`.

## 4. Extensibility
New widgets can be "hot-swapped" or added via the registry without recompiling the main dashboard shell.
