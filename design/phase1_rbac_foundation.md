# Phase 1: Core Infrastructure & RBAC Foundation

## Objective
Establish a robust Role-Based Access Control (RBAC) system that governs all user interactions, UI visibility, and contextual action permissions.

## 1. Database Schema
### Tables
- **`roles`**: Defines system roles (Admin, Faculty, Mentor, Evaluator, Student).
- **`permission_groups`**: Categorizes permissions for UI manageability (e.g., User Management, Evaluation, Internship).
- **`permissions`**: Granular keys (e.g., `create_faculty`, `assign_mentor`) linked to groups.
- **`role_permissions`**: Matrix linking roles to allowed permissions.
- **`user_roles`**: Links Supabase Auth users to one or more roles.
- **`user_permission_override`**: Explicitly enable/disable a specific permission for a specific user.

### NEW: Action Permissions
RBAC controls visibility, but **Contextual Workflow Permissions** control execution.
- **`workflow_permissions`**:
  - `role_id`
  - `stage_id`
  - `action` (e.g., `approve_report`, `finalize_grade`)
  - `allowed` (boolean)

## 2. Technical Implementation
### `PermissionService.cs`
- **Caching**: Store user permissions in a thread-safe `HashSet<string>` upon login for $O(1)$ lookup.
- **Combined Check**:
  ```csharp
  bool HasAccess = GetRbacPermission(user, "evaluate_submission") && 
                 GetWorkflowPermission(role, currentStage, "finalize_grade");
  ```

### `PermissionView.razor`
A wrapper component:
```razor
<PermissionView Permission="create_faculty">
    <MudButton>Create Faculty</MudButton>
</PermissionView>
```

## 3. Security Requirements
- **Server-Side Validation**: Every service call must re-verify permissions before execution.
- **Audit Logs**: Every permission-check failure and administrative role change must be logged to `audit_logs`.
