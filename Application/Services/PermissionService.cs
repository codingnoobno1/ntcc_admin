using Microsoft.AspNetCore.Components.Authorization;
using ntcc_admin_blazor.Domain.Entities;
using ntcc_admin_blazor.Services;
using System.Security.Claims;

namespace ntcc_admin_blazor.Application.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly SupabaseService _supabase;
        private readonly AuthenticationStateProvider _authStateProvider;
        private HashSet<string> _cachedPermissions;
        private Guid? _currentUserId;

        public PermissionService(SupabaseService supabase, AuthenticationStateProvider authStateProvider)
        {
            _supabase = supabase;
            _authStateProvider = authStateProvider;
        }

        public async Task<bool> HasPermissionAsync(string permissionKey)
        {
            var permissions = await GetUserPermissionsAsync();
            return permissions.Contains(permissionKey);
        }

        public async Task<bool> HasWorkflowActionAsync(Guid stageId, string action)
        {
            var userId = await GetUserIdAsync();
            if (userId == null) return false;

            try
            {
                // 1. Get User Roles
                var userRoles = await _supabase.GetWhere<UserRoleEntity>("user_id", userId.Value);
                
                // 2. Check Workflow Permissions for these roles
                foreach (var userRole in userRoles)
                {
                    var workflowPerms = await _supabase.GetWhere<WorkflowPermissionEntity>("role_id", userRole.RoleId);
                    if (workflowPerms.Any(p => p.StageId == stageId && p.Action == action && p.Allowed))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RBAC Warning] Workflow permission check failed (tables may be missing): {ex.Message}");
                // Fallback for master/admin if needed, or simply return false
            }

            return false;
        }

        public async Task<HashSet<string>> GetUserPermissionsAsync()
        {
            if (_cachedPermissions != null) return _cachedPermissions;

            var userId = await GetUserIdAsync();
            if (userId == null) return new HashSet<string>();

            _cachedPermissions = new HashSet<string>();

            try
            {
                // 1. Get Roles for User
                var userRoles = await _supabase.GetWhere<UserRoleEntity>("user_id", userId.Value);
                
                // 2. Get Permissions for these Roles
                foreach (var userRole in userRoles)
                {
                    var rolePerms = await _supabase.GetWhere<RolePermissionEntity>("role_id", userRole.RoleId);
                    var enabledPerms = rolePerms.Where(p => p.Enabled);
                    
                    foreach (var rp in enabledPerms)
                    {
                        var perms = await _supabase.GetWhere<PermissionEntity>("id", rp.PermissionId);
                        var perm = perms.FirstOrDefault();
                        if (perm != null) _cachedPermissions.Add(perm.PermissionKey);
                    }
                }

                // 3. Apply Overrides
                var overrides = await _supabase.GetWhere<UserPermissionOverrideEntity>("user_id", userId.Value);
                foreach (var ovr in overrides)
                {
                    var perms = await _supabase.GetWhere<PermissionEntity>("id", ovr.PermissionId);
                    var perm = perms.FirstOrDefault();
                    if (perm == null) continue;

                    if (ovr.Enabled) _cachedPermissions.Add(perm.PermissionKey);
                    else _cachedPermissions.Remove(perm.PermissionKey);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RBAC Warning] Global permission check failed (tables may be missing): {ex.Message}");
                // If tables are missing, we optionally grant some defaults based on email or let it be empty
                // For now, allow basic access if they are authenticated to bypass the crash
                _cachedPermissions.Add("view_dashboard");
            }

            return _cachedPermissions;
        }

        public async Task ClearCacheAsync()
        {
            _cachedPermissions = null;
            _currentUserId = null;
        }

        private async Task<Guid?> GetUserIdAsync()
        {
            if (_currentUserId != null) return _currentUserId;

            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var guid))
            {
                _currentUserId = guid;
                return _currentUserId;
            }

            return null;
        }
    }
}
