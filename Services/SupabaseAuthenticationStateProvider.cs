using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using ntcc_admin_blazor.Services;
using ntcc_admin_blazor.Models;

namespace ntcc_admin_blazor.Services
{
    public class SupabaseAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly SupabaseService _supabase;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SupabaseAuthenticationStateProvider(SupabaseService supabase, IHttpContextAccessor httpContextAccessor)
        {
            _supabase = supabase;
            _httpContextAccessor = httpContextAccessor;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext?.User?.Identity?.IsAuthenticated == true)
            {
                // Cookie-based: claims already exist from the cookie
                Console.WriteLine($"[AUTH-STATE] Cookie auth active. User: {httpContext.User.Identity.Name}");
                return Task.FromResult(new AuthenticationState(httpContext.User));
            }

            Console.WriteLine("[AUTH-STATE] No active session.");
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
        }

        public void NotifyAuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
