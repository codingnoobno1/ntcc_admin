# Phase 7: Internship Lifecycle Module

## Objective
Handle the external complexities of internships, including company registries and specialized verification.

## 1. External Registries
- **`companies`**: Central database of industry partners.
  - `name`, `industry`, `location`, `rating`.
- **`company_supervisors`**: Contact registry for external mentors.
  - `name`, `email`, `designation`.

## 2. Specialized Internships Table
- **`internships`**:
  - `student_id`, `company_id`, `supervisor_id`, `start_date`, `end_date`, `mode` (Remote/In-person).

## 3. Requirement Workflow
- **`internship_requirements`**: Configurable per-stage (Offer Letter, NOC, Weekly Reports).
- **Verification Portal**: Host faculty UI to audit uploaded PDFs against industry records.
- **Feedback Link**: Generates a secure, temporary token for `company_supervisors` to submit evaluations without a full account.
