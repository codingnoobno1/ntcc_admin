namespace ntcc_admin_blazor.Domain.Enums
{
    public enum StepStatus
    {
        Pending,
        Submitted,
        UnderReview,
        Approved,
        Rejected
    }

    public enum ComponentType
    {
        Metric,
        Visualization,
        Academic,
        Evaluation,
        Productivity
    }
}
