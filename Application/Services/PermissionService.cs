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

        public async Task<bool> HasWorkflowActionAsync(long stageId, string action)
        {
            var userId = await GetUserIdAsync();
            if (userId == null) return false;

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

            return false;
        }

        public async Task<HashSet<string>> GetUserPermissionsAsync()
        {
            if (_cachedPermissions != null) return _cachedPermissions;

            var userId = await GetUserIdAsync();
            if (userId == null) return new HashSet<string>();

            _cachedPermissions = new HashSet<string>();

            // 1. Get Roles for User
            var userRoles = await _supabase.GetWhere<UserRoleEntity>("user_id", userId.Value);
            
            // 2. Get Permissions for these Roles
            foreach (var userRole in userRoles)
            {
                var rolePerms = await _supabase.GetWhere<RolePermissionEntity>("role_id", userRole.RoleId);
                var enabledPerms = rolePerms.Where(p => p.Enabled);
                
                foreach (var rp in enabledPerms)
                {
                    var perm = await _supabase.GetById<PermissionEntity>(rp.PermissionId);
                    if (perm != null) _cachedPermissions.Add(perm.PermissionKey);
                }
            }

            // 3. Apply Overrides
            var overrides = await _supabase.GetWhere<UserPermissionOverrideEntity>("user_id", userId.Value);
            foreach (var ovr in overrides)
            {
                var perm = await _supabase.GetById<PermissionEntity>(ovr.PermissionId);
                if (perm == null) continue;

                if (ovr.Enabled) _cachedPermissions.Add(perm.PermissionKey);
                else _cachedPermissions.Remove(perm.PermissionKey);
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
