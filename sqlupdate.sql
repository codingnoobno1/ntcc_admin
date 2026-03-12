-- SQL Update Script for NTCC Admin Blazor
-- Synchronizes database schema with updated Domain Entities

-- 1. Create missing tables for Dashboard Components
CREATE TABLE IF NOT EXISTS step_components (
    id TEXT PRIMARY KEY DEFAULT gen_random_uuid()::text,
    step_id TEXT NOT NULL,
    component_id TEXT NOT NULL,
    created_at TIMESTAMPTZ DEFAULT NOW(),
    updated_at TIMESTAMPTZ DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS component_registry (
    id TEXT PRIMARY KEY DEFAULT gen_random_uuid()::text,
    component_key TEXT NOT NULL,
    component_path TEXT NOT NULL,
    type TEXT DEFAULT 'Metric',
    created_at TIMESTAMPTZ DEFAULT NOW(),
    updated_at TIMESTAMPTZ DEFAULT NOW()
);

-- 2. Update project_groups table with missing fields
DO $$ 
BEGIN 
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'project_groups' AND COLUMN_NAME = 'project_type') THEN
        ALTER TABLE project_groups ADD COLUMN project_type TEXT DEFAULT 'Minor';
    END IF;

    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'project_groups' AND COLUMN_NAME = 'description') THEN
        ALTER TABLE project_groups ADD COLUMN description TEXT;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'project_groups' AND COLUMN_NAME = 'semester') THEN
        ALTER TABLE project_groups ADD COLUMN semester INT4;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'project_groups' AND COLUMN_NAME = 'batch_id') THEN
        ALTER TABLE project_groups ADD COLUMN batch_id TEXT;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'project_groups' AND COLUMN_NAME = 'submission_status') THEN
        ALTER TABLE project_groups ADD COLUMN submission_status TEXT DEFAULT 'pending';
    END IF;
END $$;

-- 3. Update stage_types table
DO $$ 
BEGIN 
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'stage_types' AND COLUMN_NAME = 'program_id') THEN
        ALTER TABLE stage_types ADD COLUMN program_id TEXT;
    END IF;
END $$;

-- 4. Update workflow_stages table
DO $$ 
BEGIN 
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'workflow_stages' AND COLUMN_NAME = 'stage_key') THEN
        ALTER TABLE workflow_stages ADD COLUMN stage_key TEXT;
    END IF;
END $$;

-- 5. Update group_members table
DO $$ 
BEGIN 
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'group_members' AND COLUMN_NAME = 'project_id') THEN
        ALTER TABLE group_members ADD COLUMN project_id TEXT;
    END IF;
END $$;

-- 9. Evaluation System Tables
CREATE TABLE IF NOT EXISTS evaluation_templates (
    id TEXT PRIMARY KEY DEFAULT gen_random_uuid()::text,
    name TEXT NOT NULL,
    stage_id TEXT REFERENCES academic_stage_rules(id),
    total_marks NUMERIC(10,2) DEFAULT 0,
    created_at TIMESTAMPTZ DEFAULT NOW(),
    updated_at TIMESTAMPTZ DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS evaluation_components (
    id TEXT PRIMARY KEY DEFAULT gen_random_uuid()::text,
    template_id TEXT REFERENCES evaluation_templates(id),
    component_name TEXT NOT NULL,
    max_marks NUMERIC(10,2) NOT NULL,
    weight NUMERIC(10,2) DEFAULT 1.0,
    created_at TIMESTAMPTZ DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS student_evaluations_v2 (
    id TEXT PRIMARY KEY DEFAULT gen_random_uuid()::text,
    student_id TEXT NOT NULL,
    group_id TEXT NOT NULL,
    template_id TEXT REFERENCES evaluation_templates(id),
    evaluator_id TEXT,
    total_score NUMERIC(10,2),
    comments TEXT,
    evaluation_date TIMESTAMPTZ DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS evaluation_scores (
    id TEXT PRIMARY KEY DEFAULT gen_random_uuid()::text,
    evaluation_id TEXT REFERENCES student_evaluations_v2(id),
    component_id TEXT REFERENCES evaluation_components(id),
    score NUMERIC(10,2) NOT NULL,
    created_at TIMESTAMPTZ DEFAULT NOW()
);

-- 6. Update student_evaluations marks to support decimals
ALTER TABLE student_evaluations ALTER COLUMN marks TYPE NUMERIC(10,2);

-- 7. Convert academic_stage_rules columns to TEXT to support custom IDs and custom text entry
ALTER TABLE academic_stage_rules ALTER COLUMN program_id TYPE TEXT;
ALTER TABLE academic_stage_rules ALTER COLUMN stage_type_id TYPE TEXT;

-- 8. Resolve RLS policy violations for academic_stage_rules
-- Allowing authenticated users full access (Standard for Admin/Faculty roles)
ALTER TABLE academic_stage_rules ENABLE ROW LEVEL SECURITY;

DO $$ 
BEGIN 
    IF NOT EXISTS (SELECT 1 FROM pg_policies WHERE tablename = 'academic_stage_rules' AND policyname = 'Allow authenticated all') THEN
        CREATE POLICY "Allow authenticated all" ON academic_stage_rules FOR ALL TO authenticated USING (true) WITH CHECK (true);
    END IF;

    -- Add stage_type column if not exists
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'academic_stage_rules' AND COLUMN_NAME = 'stage_type') THEN
        ALTER TABLE academic_stage_rules ADD COLUMN stage_type TEXT DEFAULT 'Minor';
    END IF;
END $$;

-- 10. Synchronize Students Table
DO $$ 
BEGIN 
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'students' AND COLUMN_NAME = 'created_at') THEN
        ALTER TABLE students ADD COLUMN created_at TIMESTAMPTZ DEFAULT NOW();
    END IF;

    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'students' AND COLUMN_NAME = 'updated_at') THEN
        ALTER TABLE students ADD COLUMN updated_at TIMESTAMPTZ DEFAULT NOW();
    END IF;

    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'students' AND COLUMN_NAME = 'batch_id') THEN
        ALTER TABLE students ADD COLUMN batch_id TEXT;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'students' AND COLUMN_NAME = 'university_id') THEN
        ALTER TABLE students ADD COLUMN university_id TEXT;
    END IF;

    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'students' AND COLUMN_NAME = 'enrollment_number') THEN
        ALTER TABLE students ADD COLUMN enrollment_number TEXT;
    END IF;
END $$;
