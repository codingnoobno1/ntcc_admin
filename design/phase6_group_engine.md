# Phase 6: Project Formation & Group Engine

## Objective
Manage the collaborative formation of project teams with strict constraints and approval workflows.

## 1. Domain Entities & Constraints
- **`project_groups`**: The team container.
- **`group_members`**: Link students to groups.
  - **Constraint**: A student cannot belong to multiple `active` project groups (handled via database triggers or service-layer validation).
- **`group_proposals`**:
  - `status`: `draft`, `pending`, `approved`, `rejected`.

## 2. Workflow & Conflict Resolution
1. **Proposal Phase**: Leaders invite members.
2. **Conflict Check**: Members must "Accept" invitation; system blocks acceptance if student is already in a group.
3. **Submission**: Topic/Title abstract sent to Mentor.
4. **Approval**: Once approved, group structure is **Locked**; no further membership changes allowed without Coordinator override.

## 3. UI Components
- `TeamBuilder`: Real-time group search and invitation portal.
- `ProposalVetPanel`: Grid view for coordinators to approve/reject titles in bulk.
