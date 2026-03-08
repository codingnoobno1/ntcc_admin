-- 1. Create Dashboards Table
CREATE TABLE dashboards (
    id BIGINT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    role TEXT UNIQUE NOT NULL,
    name TEXT NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT timezone('utc'::text, now()) NOT NULL,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT timezone('utc'::text, now()) NOT NULL
);

-- 2. Create Dashboard Widgets Table
CREATE TABLE dashboard_widgets (
    id BIGINT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    dashboard_id BIGINT REFERENCES dashboards(id) ON DELETE CASCADE,
    component_key TEXT NOT NULL,
    display_order INT NOT NULL DEFAULT 0,
    width INT NOT NULL DEFAULT 12,
    config_json JSONB DEFAULT '{}'::jsonb,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT timezone('utc'::text, now()) NOT NULL,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT timezone('utc'::text, now()) NOT NULL
);

-- 3. Create Custom Nav Items Table
CREATE TABLE custom_nav_items (
    id BIGINT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    role TEXT NOT NULL,
    label TEXT NOT NULL,
    route TEXT NOT NULL,
    icon TEXT,
    order_index INT NOT NULL DEFAULT 0,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT timezone('utc'::text, now()) NOT NULL,
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT timezone('utc'::text, now()) NOT NULL
);


-- ==========================================
-- SEED DATA
-- ==========================================

-- Seed Faculty Dashboard (Extending the default ones defined in FacultyDashboard.razor)
INSERT INTO dashboards (role, name) VALUES ('faculty', 'Faculty Dashboard');

-- Insert additional widgets for faculty through the DB
INSERT INTO dashboard_widgets (dashboard_id, component_key, display_order, width)
VALUES ((SELECT id FROM dashboards WHERE role = 'faculty'), 'project_supervision', 3, 12);

-- Seed Custom Dashboard: Dean
INSERT INTO dashboards (role, name) VALUES ('dean', 'Dean Dashboard');

-- Insert custom widgets for the Dean
INSERT INTO dashboard_widgets (dashboard_id, component_key, display_order, width)
VALUES 
    ((SELECT id FROM dashboards WHERE role = 'dean'), 'department_metrics', 1, 12),
    ((SELECT id FROM dashboards WHERE role = 'dean'), 'research_publications', 2, 6),
    ((SELECT id FROM dashboards WHERE role = 'dean'), 'student_success_rate', 3, 6);

-- Seed Custom Nav for Dean
INSERT INTO custom_nav_items (role, label, route, icon, order_index)
VALUES
    ('dean', 'Department Metrics', 'metrics', '@Icons.Material.Filled.Analytics', 1),
    ('dean', 'Research Reports', 'research', '@Icons.Material.Filled.MenuBook', 2);

-- Seed Custom Dashboard: Placement
INSERT INTO dashboards (role, name) VALUES ('placement', 'Placement Dashboard');

-- Insert custom widgets for Placement
INSERT INTO dashboard_widgets (dashboard_id, component_key, display_order, width)
VALUES 
    ((SELECT id FROM dashboards WHERE role = 'placement'), 'internship_pipeline', 1, 12),
    ((SELECT id FROM dashboards WHERE role = 'placement'), 'company_registry_widget', 2, 6),
    ((SELECT id FROM dashboards WHERE role = 'placement'), 'offer_tracker', 3, 6);

-- Seed Custom Nav for Placement
INSERT INTO custom_nav_items (role, label, route, icon, order_index)
VALUES
    ('placement', 'Company Registry', 'admin/companies', '@Icons.Material.Filled.Business', 1),
    ('placement', 'Internship Pipeline', 'internships/pipeline', '@Icons.Material.Filled.Timeline', 2),
    ('placement', 'Offer Tracker', 'internships/offers', '@Icons.Material.Filled.LocalOffer', 3);

-- Seed Custom Dashboard: Evaluator
INSERT INTO dashboards (role, name) VALUES ('evaluator', 'Evaluator Dashboard');

-- Insert custom widgets for Evaluator
INSERT INTO dashboard_widgets (dashboard_id, component_key, display_order, width)
VALUES 
    ((SELECT id FROM dashboards WHERE role = 'evaluator'), 'evaluation_queue', 1, 12),
    ((SELECT id FROM dashboards WHERE role = 'evaluator'), 'rubric_scoring', 2, 12);

-- Seed Custom Nav for Evaluator
INSERT INTO custom_nav_items (role, label, route, icon, order_index)
VALUES
    ('evaluator', 'Pending Evaluations', 'evaluation-panel', '@Icons.Material.Filled.PendingActions', 1),
    ('evaluator', 'Evaluation History', 'evaluation-history', '@Icons.Material.Filled.History', 2);
