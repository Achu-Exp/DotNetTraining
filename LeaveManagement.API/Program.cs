using System.Text;
using Microsoft.OpenApi.Models;
using FluentValidation.AspNetCore;
using LeaveManagement.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using LeaveManagement.Infrastructure;
using LeaveManagement.Domain.Entities;
using LeaveManagement.Application.Services;
using LeaveManagement.Application.Validators;
using LeaveManagementSystem.Application.Services;
using LeaveManagement.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using LeaveManagement.Application.Services.Interfaces;
using LeaveManagement.Infrastructure.Repositories.Interfaces;

DotNetEnv.Env.Load();

var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
var jwt_Scret = Environment.GetEnvironmentVariable("API_SECRET");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddScoped<IManagerSevice, ManagerService>();
builder.Services.AddScoped<IManagerRepository, ManagerRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

builder.Services.AddSingleton<LeaveRequestNotifier>();

builder.Services.AddScoped<ILeaveRequestService, LeaveRequestService>();
builder.Services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddSwaggerGen(option =>

{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
             "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
             "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
             "Example: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
            jwt_Scret)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ManagerPolicy", policy => policy.RequireRole("Manager"));
    options.AddPolicy("EmployeePolicy", policy => policy.RequireRole("Employee"));
});


builder.Services.AddFluentValidationAutoValidation(); // Enables auto-validation

// Register validators
builder.Services.AddValidatorsFromAssemblyContaining<UserDtoValidator>(); 
builder.Services.AddValidatorsFromAssemblyContaining<EmployeeDtoValidator>(); 
builder.Services.AddValidatorsFromAssemblyContaining<ManagerDtoValidator>(); 
builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestDtoValidator>(); 
builder.Services.AddValidatorsFromAssemblyContaining<DepartmentDtoValidator>(); 
builder.Services.AddValidatorsFromAssemblyContaining<LeaveRequestDtoValidator>(); 

var app = builder.Build();

// Subscribe to LeaveRequestCreated event - whereever LeaveRequestCreated is raised, runs this code
var notifier = app.Services.GetRequiredService<LeaveRequestNotifier>();

// sender is the object that raises the event
// e is the event arguments
notifier.LeaveRequestCreated += async (sender, e) =>
{
    using var scope = app.Services.CreateScope();
    var employeeRepo = scope.ServiceProvider.GetRequiredService<IEmployeeRepository>();
    var managerRepo = scope.ServiceProvider.GetRequiredService<IManagerRepository>();
    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

    var employee = await employeeRepo.FindAsync(e.EmployeeId);
    var approver = await managerRepo.FindAsync(e.ApproveId);

    if (approver != null && employee != null)
    {
        string subject = "New Leave Request";
        string body = $"Hi {approver.User.Name},\n\n" +
                      $"Employee {employee.User.Name} has submitted a leave request.";

        await emailService.SendEmail(approver.User.Email, subject, body);
        await emailService.SendEmail(employee.User.Email, "Leave Request", "The leave request has been submitted successfully!");
    }
};

notifier.LeaveStatusChanged += async (sender, e) =>
{
    using var scope = app.Services.CreateScope();
    var employeeRepo = scope.ServiceProvider.GetRequiredService<IEmployeeRepository>();
    var managerRepo = scope.ServiceProvider.GetRequiredService<IManagerRepository>();
    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

    var employee = await employeeRepo.FindAsync(e.EmployeeId);
    var approver = await managerRepo.FindAsync(e.ApproverId.Value); // ApproverId is nullable
    if (approver != null && employee != null)
    {
        string subject = "";
        string body = "";

        // Handle the email notification based on leave request status
        if (e.Status == LeaveStatus.Approved)
        {
            subject = "Your Leave Request has been Approved";
            body = $"Dear {employee.User.Name},\n\n" +
                   $"Your leave request from {e.StartDate} to {e.EndDate} has been approved." +
                   $"Enjoy your time off!";
        }
        else if (e.Status == LeaveStatus.Rejected)
        {
            subject = "Your Leave Request has been Rejected";
            body = $"Dear {employee.User.Name},\n\n" +
                   $"Unfortunately, your leave request from {e.StartDate} to {e.EndDate} has been rejected." +
                   $"Please contact your manager for more details.";
        }
    }
};

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
