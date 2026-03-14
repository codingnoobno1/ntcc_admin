using ntcc_admin_blazor.Models;
using Supabase;
using Postgrest.Models;
using Supabase.Gotrue;

namespace ntcc_admin_blazor.Services
{
    /// <summary>
    /// Central data access layer between Blazor components and Supabase (PostgREST + Auth),
    /// consumed by application services for authentication, generic CRUD, domain queries,
    /// and admin user creation.
    /// </summary>
    /// <remarks>
    /// Typical flow: service-layer request and models come in, the Supabase client is
    /// initialized on first use, a PostgREST query is executed, and results are deserialized
    /// into domain entities or auth sessions.
    /// </remarks>
    public class SupabaseService
    {
        public Supabase.Client Client { get; private set; } = null!;
        private readonly IConfiguration _configuration;
        private bool _initialized;

        public SupabaseService(IConfiguration configuration)
        {
            _configuration = configuration;
            Initialize();
        }

        private void Initialize()
        {
            var url = _configuration["Supabase:Url"];
            var key = _configuration["Supabase:AnonKey"];

            var options = new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
            };

            Client = new Supabase.Client(url!, key!, options);
        }

        public async Task InitializeAsync()
        {
            if (_initialized)
                return;

            await Client.InitializeAsync();
            _initialized = true;
        }

        // ─── Auth ────────────────────────────────────────

        public async Task<Session?> SignIn(string email, string password)
        {
            try
            {
                var session = await Client.Auth.SignIn(email, password);
                return session;
            }
            catch
            {
                return null;
            }
        }

        public async Task<User?> SignUp(string email, string password, string role, string fullName)
        {
            try
            {
                var options = new SignUpOptions
                {
                    Data = new Dictionary<string, object>
                    {
                        { "role", role },
                        { "full_name", fullName }
                    }
                };

                var session = await Client.Auth.SignUp(email, password, options);
                return session?.User;
            }
            catch
            {
                return null;
            }
        }

        public async Task SignOut()
        {
            if (Client.Auth.CurrentSession != null)
                await Client.Auth.SignOut();
        }

        public Session? CurrentSession => Client.Auth.CurrentSession;

        // ─── Generic CRUD ────────────────────────────────

        public async Task<List<T>> GetAll<T>() where T : BaseModel, new()
        {
            await InitializeAsync();
            var result = await Client.From<T>().Get();
            return result.Models;
        }

        public async Task<List<T>> GetWhere<T>(string column, object? value) where T : BaseModel, new()
        {
            await InitializeAsync();

            var result = await Client
                .From<T>()
                .Filter(column, Postgrest.Constants.Operator.Equals, value?.ToString())
                .Get();

            return result.Models;
        }

        public async Task<T?> GetById<T>(object id) where T : BaseModel, new()
        {
            await InitializeAsync();

            var result = await Client
                .From<T>()
                .Filter("id", Postgrest.Constants.Operator.Equals, id?.ToString())
                .Get();

            return result.Models.FirstOrDefault();
        }

        public async Task<T> Insert<T>(T model) where T : BaseModel, new()
        {
            await InitializeAsync();
            var result = await Client.From<T>().Insert(model);
            return result.Models.First();
        }

        public async Task<T> Update<T>(T model) where T : BaseModel, new()
        {
            await InitializeAsync();
            var result = await Client.From<T>().Update(model);
            return result.Models.First();
        }

        public async Task<T?> Upsert<T>(T model) where T : BaseModel, new()
        {
            await InitializeAsync();
            var result = await Client.From<T>().Upsert(model);
            return result.Models.FirstOrDefault();
        }

        public async Task Delete<T>(T model) where T : BaseModel, new()
        {
            await InitializeAsync();
            await Client.From<T>().Delete(model);
        }

        // ─── Specialized Domain Queries ──────────────────

        public async Task<List<NtccStage>> GetStagesForFaculty(Guid facultyId, string? role = null)
        {
            await InitializeAsync();

            var facultyLinks = await Client
                .From<StageFaculty>()
                .Where(x => x.FacultyId == facultyId)
                .Get();

            var stageIds = facultyLinks.Models
                .Where(x => role == null || x.Role == role)
                .Select(x => x.StageId)
                .ToList();

            var stages = await Client.From<NtccStage>().Get();

            return stages.Models.Where(s => stageIds.Contains(s.Id)).ToList();
        }

        public async Task<(List<StageDeadline>, List<EvaluationCategory>, List<StageRequirement>, List<StageSubmissionRule>)>
            GetStageConfiguration(Guid stageId)
        {
            await InitializeAsync();

            var deadlines = await Client.From<StageDeadline>().Where(x => x.StageId == stageId).Get();
            var categories = await Client.From<EvaluationCategory>().Where(x => x.StageId == stageId).Get();
            var requirements = await Client.From<StageRequirement>().Where(x => x.StageId == stageId).Get();
            var submissionRules = await Client.From<StageSubmissionRule>().Where(x => x.StageId == stageId).Get();

            return (
                deadlines.Models,
                categories.Models,
                requirements.Models,
                submissionRules.Models
            );
        }

        public async Task<List<EvaluationComponent>> GetEvaluationComponents(Guid categoryId)
        {
            await InitializeAsync();

            var result = await Client
                .From<EvaluationComponent>()
                .Where(x => x.CategoryId == categoryId)
                .Get();

            return result.Models;
        }

        // ─── Faculty Management ──────────────────────────

        public static string GenerateStrongPassword()
        {
            const string chars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            var random = new Random();

            return new string(
                Enumerable.Repeat(chars, 12)
                .Select(s => s[random.Next(s.Length)])
                .ToArray()
            );
        }

        public async Task<Guid?> CreateFacultyAccount(
            string email,
            string password,
            string fullName,
            string department,
            List<string> roles)
        {
            try
            {
                await InitializeAsync();

                var adminClient = GetAdminClient();
                if (adminClient == null)
                    throw new Exception("ServiceRoleKey missing");

                var userAttributes = new AdminUserAttributes
                {
                    Email = email,
                    Password = password,
                    EmailConfirm = true,
                    Data = new Dictionary<string, object>
                    {
                        { "full_name", fullName },
                        { "role", "faculty" },
                        { "department", department }
                    }
                };

                var response = await ((dynamic)adminClient.Auth).Admin.CreateUser(userAttributes);

                string userIdStr = response?.Id;

                if (string.IsNullOrEmpty(userIdStr) || !Guid.TryParse(userIdStr, out Guid userId))
                    throw new Exception("Invalid user ID returned");

                var profile = new Profile
                {
                    Id = userId,
                    Email = email,
                    FullName = fullName,
                    Role = "faculty",
                    Department = department,
                    IsVerified = true
                };

                await Insert(profile);

                foreach (var role in roles)
                {
                    await Insert(new FacultyRole
                    {
                        FacultyId = userId,
                        Role = role.ToLower()
                    });
                }

                return userId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FACULTY-CREATION ERROR] {ex.Message}");
                throw;
            }
        }

        private Supabase.Client? GetAdminClient()
        {
            var url = _configuration["Supabase:Url"];
            var serviceKey = _configuration["Supabase:ServiceRoleKey"];

            if (string.IsNullOrEmpty(serviceKey))
                return null;

            return new Supabase.Client(url!, serviceKey,
                new SupabaseOptions { AutoRefreshToken = true });
        }
    }
}