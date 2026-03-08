namespace ntcc_admin_blazor.Application.Services
{
    public interface IPermissionService
    {
        Task<bool> HasPermissionAsync(string permissionKey);
        Task<bool> HasWorkflowActionAsync(long stageId, string action);
        Task<HashSet<string>> GetUserPermissionsAsync();
        Task ClearCacheAsync();
    }
}
