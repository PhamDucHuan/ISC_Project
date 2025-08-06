using ISC_Project.Data;
using ISC_Project.Hubs;
using ISC_Project.Infrastructure.Services;
using ISC_Project.Interface;
using ISC_Project.Interface.AuthService;
using ISC_Project.Interface.Exam;
using ISC_Project.Services;
using ISC_Project.Services.AuthService;
using ISC_Project.Services.Exam;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;

namespace ISC_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Set switch to enable legacy timestamp behavior for Npgsql
            // This is necessary to handle timestamp with time zone correctly in PostgreSQL
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            builder.Services.AddSignalR();

            #region Register services into the IoC container
            // Register services into the IoC container
            builder.Services.AddScoped<ISC_ProjectDbContext, ISC_ProjectDbContext>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IJwtService, JwtService>();

            // Registering various services
            builder.Services.AddScoped<IExamService, ExamService>();

            builder.Services.AddScoped<IAcceptingSchoolTransferService, AcceptingSchoolTransferService>();
            builder.Services.AddScoped<IAssessmentPartService, AssessmentPartService>();
            builder.Services.AddScoped<IAssessmentQuestionService, AssessmentQuestionService>();
            builder.Services.AddScoped<IAssignmentGroupService, AssignmentGroupService>();
            builder.Services.AddScoped<IAssignmentService, AssignmentService>();
            builder.Services.AddScoped<ICampusService, CampusService>();
            builder.Services.AddScoped<IChatAIService, ChatAIService>();

            builder.Services.AddScoped<IClassDetailService, ClassDetailService>();
            builder.Services.AddScoped<IClassHistoryService, ClassHistoryService>();
            builder.Services.AddScoped<IClassroomSettingService, ClassroomSettingService>();
            builder.Services.AddScoped<IClassService, ClassService>();
            builder.Services.AddScoped<IClassSubjectService, ClassSubjectService>();
            builder.Services.AddScoped<IClassTypeService, ClassTypeService>();

            builder.Services.AddScoped<ICourseCategoryService, CourseCategoryService>();
            builder.Services.AddScoped<ICourseLessonService, CourseLessonService>();
            builder.Services.AddScoped<ICourseOfferingService, CourseOfferingService>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<ICoursesLearnedService, CoursesLearnedService>();

            builder.Services.AddScoped<IDiscussionThreadService, DiscussionThreadService>();
            builder.Services.AddScoped<IEmploymentHistoryService, EmploymentHistoryService>();
            builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
            builder.Services.AddScoped<IExemptionService, ExemptionService>();
            builder.Services.AddScoped<IExportStudentService, ExportStudentService>();
            builder.Services.AddScoped<IFacultyStudyBlockService, FacultyStudyBlockService>();

            builder.Services.AddScoped<IFamilyInformationService, FamilyInformationService>();
            builder.Services.AddScoped<IGradeService, GradeService>();
            builder.Services.AddScoped<ILearningOutcomeService, LearningOutcomeService>();
            builder.Services.AddScoped<ILabGraderService, LabGraderService>();
            builder.Services.AddScoped<ILiveChatMessageService, LiveChatMessageService>();
            builder.Services.AddScoped<ILiveSessionService, LiveSessionService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();

            builder.Services.AddScoped<IPastClassesService, PastClassesService>();
            builder.Services.AddScoped<IPermissionService, PermissionService>();
            builder.Services.AddScoped<IPrivateChatService, PrivateChatService>();
            builder.Services.AddScoped<IQualificationsService, QualificationService>();
            builder.Services.AddScoped<IQuestionBankService, QuestionBankService>();

            builder.Services.AddScoped<IRegistrationService, RegistrationService>();
            builder.Services.AddScoped<IRelativesInformationService, RelativesInformationService>();
            builder.Services.AddScoped<IReservedService, ReservedService>();
            builder.Services.AddScoped<IRewardDisciplineService, RewardDisciplineService>();
            builder.Services.AddScoped<IRolePermissionService, RolePermissionService>();

            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<ISchoolProfileService, SchoolProfileService>();
            builder.Services.AddScoped<ISchoolYearService, SchoolYearService>();
            builder.Services.AddScoped<IScoreService, ScoreService>();
            builder.Services.AddScoped<ISemesterService, SemesterService>();
            builder.Services.AddScoped<IStudentProfileService, StudentProfileService>();

            builder.Services.AddScoped<IStudentsChangeClassService, StudentsChangeClassService>();
            builder.Services.AddScoped<IStudentsChangeSchoolService, StudentsChangeSchoolService>();
            builder.Services.AddScoped<ISubjectService, SubjectService>();
            builder.Services.AddScoped<ISubjectTypeService, SubjectTypeService>();
            builder.Services.AddScoped<ISyllabusTopicService, SyllabusTopicService>();

            builder.Services.AddScoped<ISystemSettingService, SystemSettingService>();
            builder.Services.AddScoped<ITeacherProfileService, TeacherProfileService>();
            builder.Services.AddScoped<ITeamDepartmentService, TeamDepartmentService>();
            builder.Services.AddScoped<IThreadPostService, ThreadPostService>();
            builder.Services.AddScoped<ITotalCoursesTakenService, TotalCoursesTakenService>();

            builder.Services.AddScoped<ITrainingLevelService, TrainingLevelService>();
            builder.Services.AddScoped<IUpcomingClassService, UpcomingClassService>();
            builder.Services.AddScoped<IUserManagementService, UserManagementService>();
            builder.Services.AddScoped<IWorkHistoryService, WorkHistoryService>();

            builder.Services.AddScoped<IChatService, ChatService>();
        #endregion

            // Thêm HttpClient cho ChatAI service
            builder.Services.AddHttpClient();

            // Cấu hình CORS cho phép frontend kết nối
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowProjectOrigin",
                    policy =>
                    {
                        // Lấy URL từ file cấu hình appsettings.json
                        var allowedOrigin = builder.Configuration["AllowedHosts"];
                        if (allowedOrigin != null && allowedOrigin != "*")
                        {
                            // Cho phép các nguồn gốc cụ thể từ file cấu hình
                            // Điều này tốt hơn cho môi trường production
                            policy.WithOrigins(allowedOrigin.Split(';'))
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials(); // Rất quan trọng cho SignalR
                        }
                        else
                        {
                            // Cho phép bất kỳ nguồn gốc nào nếu không có cấu hình (dành cho development)
                            // Cảnh báo: Không khuyến khích cho production
                            policy.AllowAnyOrigin()
                                  .AllowAnyHeader()
                                  .AllowAnyMethod();
                        }
                    });
            });

            // 1. Setup DbContext to use PostgresSQL
            var connectionJwtString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<ISC_ProjectDbContext>(options =>
                options.UseNpgsql(connectionJwtString));

            //2. Setup JWT Authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();

                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";


                            var result = JsonSerializer.Serialize(new { message = "Vui lòng đăng nhập để thực hiện chức năng này." });
                            return context.Response.WriteAsync(result);
                        }
                    };
                });

            //Thêm hỗ trợ cho Newtonsoft.Json để xử lý Enum dưới dạng string và các vòng lặp tham chiếu
            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                // Cấu hình thông tin cơ bản cho Swagger
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "ISC Project API", Version = "v1" });

                // 1. Định nghĩa Security Scheme (phương thức bảo mật)
                // Dòng này khai báo cho Swagger biết có một phương thức bảo mật tên là 'Bearer'
                // sử dụng JWT thông qua Header 'Authorization'.
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Vui lòng nhập Access Token với tiền tố 'Bearer ' vào ô bên dưới. Ví dụ: 'Bearer a1b2c3d4e5'",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                // 2. Thêm Security Requirement
                // Dòng này yêu cầu Swagger phải áp dụng Security Scheme đã định nghĩa ở trên
                // cho tất cả các API cần xác thực.
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.InjectJavascript("/swagger-custom.js");
                });
            }

            app.UseHttpsRedirection();

            // Sử dụng CORS
            app.UseCors("AllowProjectOrigin");

            // Serve static files
            app.UseStaticFiles();

            app.UseStatusCodePages(async context =>
            {
                // Nếu mã lỗi là 403 Forbidden
                if (context.HttpContext.Response.StatusCode == 403)
                {
                    context.HttpContext.Response.ContentType = "application/json";
                    await context.HttpContext.Response.WriteAsJsonAsync(new { message = "Bạn không có quyền truy cập chức năng này." });
                }
            });
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Chat}/{id?}"); // Đặt Chat làm trang mặc định


            // Add routing for the root path to redirect to home.html
            app.MapGet("/", context =>
            {
                context.Response.Redirect("/Menu.html");
                return Task.CompletedTask;
            });

            // simple reverse proxy for chat
            app.MapGet("/chat", async context =>
            {
                try
                {
                    var httpClient = context.RequestServices.GetRequiredService<IHttpClientFactory>().CreateClient();
                    var response = await httpClient.GetAsync("http://localhost:3000");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        context.Response.ContentType = "text/html; charset=utf-8";
                        await context.Response.WriteAsync(content);
                    }
                    else
                    {
                        context.Response.StatusCode = 503;
                        await context.Response.WriteAsync("Chat service không khả dụng");
                    }
                }
                catch
                {
                    context.Response.StatusCode = 503;
                    context.Response.ContentType = "text/html; charset=utf-8";
                    await context.Response.WriteAsync(@"
                        <!DOCTYPE html>
                        <html>
                        <head><title>Chat Service Error</title></head>
                        <body>
                            <h1>🚫 Chat Service Không Khả Dụng</h1>
                            <p>Không thể kết nối đến http://localhost:3000</p>
                            <p><a href='/'>← Quay về trang chủ</a></p>
                        </body>
                        </html>");
                }
            });

            app.MapHub<PrivateChatHub>("/privatechathub");

            app.MapControllers();
            app.Run();
        }
    }
}
