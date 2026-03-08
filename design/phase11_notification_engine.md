# Phase 11: Notification & Messaging Engine

## Objective
Ensure real-time communication of critical workflow events across all roles.

## 1. Messaging Infrastructure
- **`notifications`**:
  - `user_id`, `type` (success, warning, error, info).
  - `title`, `message`, `is_read`.
  - `target_url` (deep link to the specific workflow step).

## 2. Event Triggers (Supabase Realtime)
Automatically broadcast events when database records change:
- **`submission_created`**: Alert Mentor.
- **`evaluation_completed`**: Alert Student and Coordinator.
- **`deadline_approaching`**: Alert all students in the stage.
- **`group_invite`**: Alert student for peer collaboration.

## 3. Delivery Channels
- **In-App Toast**: Real-time popups via `MudSnackbar`.
- **Notification Center**: Bell icon with persistent unread list.
- **Email Bridge**: Integration with SMTP to forward critical alerts.
