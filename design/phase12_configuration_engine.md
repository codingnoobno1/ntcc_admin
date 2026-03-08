# Phase 12: Global Configuration Engine

## Objective
Centralize all engine-level parameters and system rules to avoid "magic numbers" in the codebase.

## 1. System Settings Registry
- **`system_settings`**:
  - `key`: `max_group_size`, `attendance_threshold`, `temp_password_expiry`.
  - `value`: (text/JSON).
  - `category`: (Academic, UI, Security).

## 2. Admin Control Panel
- A dedicated "Settings" page for Super-Admins to:
  - Toggle global features (e.g., Enable/Disable registration).
  - Configure grading thresholds (e.g., Grade A = 90% and above).
  - Set email templates and SMTP configurations.

## 3. Cache & Precedence
- Settings are loaded into memory at app startup and updated via Realtime hooks to ensure zero-latency lookups across the system logic.
