using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ntcc_admin_blazor.Components;
using ntcc_admin_blazor.Services;
using ntcc_admin_blazor.Models;
using ntcc_admin_blazor.Application.Services;
using System.Security.Claims;
using FluentValidation;
using MediatR;
using EasyCaching.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthenticationStateProvider, SupabaseAuthenticationStateProvider>();

// Cookie Authentication: Required for [Authorize] to work
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/adminlogin";
        options.AccessDeniedPath = "/";
        options.ExpireTimeSpan = TimeSpan.FromHours(24);
    });

builder.Services.AddAuthorization();
builder.Services.AddSingleton<SupabaseService>();
builder.Services.AddScoped<IStudentAppService, StudentAppService>();
builder.Services.AddScoped<IFacultyAppService, FacultyAppService>();
builder.Services.AddScoped<IStageAppService, StageAppService>();
builder.Services.AddScoped<IWorkflowService, WorkflowService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IEvaluationService, EvaluationService>();
builder.Services.AddScoped<IActivityLogService, ActivityLogService>();
builder.Services.AddScoped<IProjectFormationService, ProjectFormationService>();
builder.Services.AddScoped<IInternshipService, InternshipService>();
builder.Services.AddMudServices();

// Enterprise Layer Integrations
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddEasyCaching(options => {
    options.UseInMemory("default");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

// ─── Auth API Endpoints ────────────────────────────────
// These handle cookie creation/destruction since HttpContext
// is not available inside Blazor Server interactive components.

app.MapGet("/api/auth/login", async (HttpContext context, string email, string role, string userId) =>
{
    Console.WriteLine($"[COOKIE] Creating session cookie for: {email} | Role: {role}");
    
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, email),
        new Claim(ClaimTypes.Email, email),
        new Claim(ClaimTypes.NameIdentifier, userId),
        new Claim(ClaimTypes.Role, role)
    };

    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    var principal = new ClaimsPrincipal(identity);

    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    
    Console.WriteLine($"[COOKIE] Session cookie created. Redirecting for role: {role}");
    if (role == "admin" || role == "master")
    {
        context.Response.Redirect("/admin/dashboard");
    }
    else if (role == "faculty")
    {
        context.Response.Redirect("/faculty/dashboard");
    }
    else
    {
        context.Response.Redirect("/dashboard");
    }
});

app.MapGet("/api/auth/logout", async (HttpContext context) =>
{
    Console.WriteLine("[COOKIE] Clearing session cookie.");
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    context.Response.Redirect("/");
});

// ─── Blazor Routes ─────────────────────────────────────
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
