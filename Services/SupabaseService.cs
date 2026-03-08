using ntcc_admin_blazor.Models;
using Supabase;
using Postgrest.Models;
using Supabase.Gotrue;

namespace ntcc_admin_blazor.Services
{
    public class SupabaseService
    {
        public Supabase.Client Client { get; private set; } = null!;
        private readonly IConfiguration _configuration;

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

            Client = new Supabase.Client(url!, key, options);
        }

        public async Task InitializeAsync()
        {
            await Client.InitializeAsync();
        }

        // ─── Auth ────────────────────────────────────────

        public async Task<Supabase.Gotrue.Session?> SignIn(string email, string password)
        {
            try
            {
                Console.WriteLine($"[AUTH] Attempting SignIn for: {email}");
                var session = await Client.Auth.SignIn(email, password);
                Console.WriteLine($"[AUTH] SignIn Result: {session != null}");
                return session;
            }
            catch (Exception ex) 
            { 
                Console.WriteLine($"[AUTH ERROR] SignIn Failed: {ex.Message}");
                return null; 
            }
        }

        public async Task<Supabase.Gotrue.User?> SignUp(string email, string password, string role, string fullName)
        {
            try
            {
                Console.WriteLine($"[AUTH] Attempting SignUp for: {email} with role: {role}");
                var options = new Supabase.Gotrue.SignUpOptions
                {
                    Data = new Dictionary<string, object> 
                    { 
                        { "role", role },
                        { "full_name", fullName }
                    }
                };
                var session = await Client.Auth.SignUp(email, password, options);
                Console.WriteLine($"[AUTH] SignUp Success. User ID: {session?.User?.Id}");
                return session?.User;
            }
            catch (Exception ex) 
            { 
                Console.WriteLine($"[AUTH ERROR] SignUp Failed: {ex.Message}");
                return null; 
            }
        }

        public async Task SignOut()
        {
            if (Client.Auth.CurrentSession != null)
                await Client.Auth.SignOut();
        }

        public Supabase.Gotrue.Session? CurrentSession => Client.Auth.CurrentSession;

        // ─── Generic CRUD ────────────────────────────────

        public async Task<List<T>> GetAll<T>() where T : BaseModel, new()
        {
            await InitializeAsync();
            var result = await Client.From<T>().Get();
            return result.Models;
        }

        public async Task<List<T>> GetWhere<T>(string column, object value) where T : BaseModel, new()
        {
            await InitializeAsync();
            var result = await Client.From<T>()
                .Filter(column, Postgrest.Constants.Operator.Equals, value?.ToString())
                .Get();
            return result.Models;
        }

        public async Task<T> Upsert<T>(T model) where T : BaseModel, new()
        {
            await InitializeAsync();
            var result = await Client.From<T>().Upsert(model);
            return result.Models.FirstOrDefault();
        }

        public async Task<T?> GetById<T>(object id) where T : BaseModel, new()
        {
            try
            {
                await InitializeAsync();
                Console.WriteLine($"[DB] GetById<{typeof(T).Name}> for id: {id}");
                var result = await Client.From<T>()
                    .Filter("id", Postgrest.Constants.Operator.Equals, id?.ToString())
                    .Get();
                Console.WriteLine($"[DB] GetById Result: {result.Models.Count} record(s) found");
                return result.Models.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB ERROR] GetById Failed: {ex.Message}");
                return default;
            }
        }

        public async Task<T> Insert<T>(T model) where T : BaseModel, new()
        {
            try
            {
                await InitializeAsync();
                Console.WriteLine($"[DB] Insert into {typeof(T).Name}");
                var result = await Client.From<T>().Insert(model);
                return result.Models.First();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB ERROR] Insert Failed: {ex.Message}");
                throw;
            }
        }

        public async Task<T> Update<T>(T model) where T : BaseModel, new()
        {
            await InitializeAsync();
            var result = await Client.From<T>().Update(model);
            return result.Models.First();
        }

        public async Task Delete<T>(T model) where T : BaseModel, new()
        {
            await InitializeAsync();
            await Client.From<T>().Delete(model);
        }

        // ─── Specialized Domain Queries ──────────────────

        public async Task<List<NtccStage>> GetStagesForFaculty(string facultyId, string? role = null)
        {
            await InitializeAsync();
            var facultyLinks = await Client.From<StageFaculty>().Where(x => x.FacultyId == facultyId).Get();
            var stageIds = facultyLinks.Models.Where(x => role == null || x.Role == role).Select(x => x.StageId).ToList();
            
            var result = await Client.From<NtccStage>().Get();
            return result.Models.Where(s => stageIds.Contains(s.Id)).ToList();
        }

        public async Task<(List<StageDeadline> Deadlines, List<EvaluationCategory> Categories, List<StageRequirement> Requirements, List<StageSubmissionRule> SubmissionRules)> GetStageConfiguration(string stageId)
        {
            await InitializeAsync();
            var deadlines = await Client.From<StageDeadline>().Where(x => x.StageId == stageId).Get();
            var categories = await Client.From<EvaluationCategory>().Where(x => x.StageId == stageId).Get();
            var requirements = await Client.From<StageRequirement>().Where(x => x.StageId == stageId).Get();
            var submissionRules = await Client.From<StageSubmissionRule>().Where(x => x.StageId == stageId).Get();
            
            return (deadlines.Models, categories.Models, requirements.Models, submissionRules.Models);
        }

        public async Task<List<EvaluationComponent>> GetEvaluationComponents(string categoryId)
        {
            await InitializeAsync();
            var result = await Client.From<EvaluationComponent>().Where(x => x.CategoryId == categoryId).Get();
            return result.Models;
        }

        // ─── Faculty Management ──────────────────────────

        public static string GenerateStrongPassword()
        {
            const string chars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 12)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<string?> CreateFacultyAccount(string email, string password, string fullName, string department, List<string> roles)
        {
            try
            {
                await InitializeAsync();
                
                // 1. Create User in Supabase Auth (Admin API)
                // Note: This requires ServiceRoleKey which we load from config
                var adminClient = GetAdminClient();
                if (adminClient == null) throw new Exception("Admin client not initialized (ServiceRoleKey missing)");

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
                var userId = (string)response?.Id;
                if (string.IsNullOrEmpty(userId)) throw new Exception("Failed to create user in Auth");

                // 2. Create Profile
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

                // 3. Assign Roles
                foreach (var role in roles)
                {
                    await Insert(new FacultyRole { FacultyId = userId, Role = role.ToLower() });
                }

                return userId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FACULTY-CREATION] Error: {ex.Message}");
                throw;
            }
        }

        private Supabase.Client? GetAdminClient()
        {
            var url = _configuration["Supabase:Url"];
            var serviceKey = _configuration["Supabase:ServiceRoleKey"];
            if (string.IsNullOrEmpty(serviceKey)) return null;

            return new Supabase.Client(url!, serviceKey, new SupabaseOptions { AutoRefreshToken = true });
        }
    }
}
