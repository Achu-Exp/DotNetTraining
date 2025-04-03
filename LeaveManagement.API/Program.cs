using LeaveManagement.Application;
using LeaveManagement.Application.Services.Interfaces;
using LeaveManagement.Application.Services;
using LeaveManagement.Infrastructure;
using LeaveManagement.Infrastructure.Repositories.Interfaces;
using LeaveManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using LeaveManagement.Application.Validators;
using FluentValidation.AspNetCore;

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

builder.Services.AddScoped<ILeaveRequestService, LeaveRequestService>();
builder.Services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddFluentValidationAutoValidation(); // Enables auto-validation

// Register validators
builder.Services.AddValidatorsFromAssemblyContaining<UserDtoValidator>(); 
builder.Services.AddValidatorsFromAssemblyContaining<EmployeeDtoValidator>(); 
builder.Services.AddValidatorsFromAssemblyContaining<ManagerDtoValidator>(); 
builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestDtoValidator>(); 
builder.Services.AddValidatorsFromAssemblyContaining<DepartmentDtoValidator>(); 
builder.Services.AddValidatorsFromAssemblyContaining<LeaveRequestDtoValidator>(); 

var app = builder.Build();

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
