-- NTCC Supabase Database Seed Script
-- Run this in the Supabase SQL Editor to organize the Admin Dashboard View

-- Ensure the row_group column exists on existing deployments
ALTER TABLE public.dashboard_widgets ADD COLUMN IF NOT EXISTS row_group text DEFAULT 'operations';

-- 1. Create the Admin Dashboard Profile
INSERT INTO public.dashboards (id, role, name, created_at)
VALUES (1, 'admin', 'Master Admin Dashboard', NOW())
ON CONFLICT (id) DO NOTHING;

-- Clear previous widget layout for a clean refresh
DELETE FROM public.dashboard_widgets WHERE dashboard_id = 1;

-- 2. Populate the Dashboard Widgets with an Organized Section Layout
INSERT INTO public.dashboard_widgets (dashboard_id, component_key, display_order, width, row_group, config_json)
VALUES 
    -- SECTION: Operations Overview (Full Width)
    (1, 'faculty_accounts', 1, 12, 'operations', '{"TotalFaculty": 54, "ActiveMentors": 32, "Evaluators": 18, "PendingInvites": 4}'),
    
    -- SECTION: Academic Insights (4 columns each)
    (1, 'student_metrics', 2, 4, 'insights', '{}'),
    (1, 'department_metrics', 3, 4, 'insights', '{}'),
    (1, 'deadline_timer', 4, 4, 'insights', '{"DaysLeft": 12}'),

    -- SECTION: Project & Internship Pipeline (6 columns each)
    (1, 'project_supervision', 5, 6, 'pipeline', '{}'),
    (1, 'internship_pipeline', 6, 6, 'pipeline', '{}'),

    -- SECTION: Administrative Tools (6 columns each)
    (1, 'evaluation_queue', 7, 6, 'admin', '{}'),
    (1, 'stage_engine', 8, 6, 'admin', '{}')
ON CONFLICT DO NOTHING;

-- Verification query:
SELECT * FROM public.dashboard_widgets WHERE dashboard_id = 1 ORDER BY display_order;
