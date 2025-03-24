using Microsoft.EntityFrameworkCore;
using Project21032025;
using Project21032025.Data;
using Project21032025.Repositories;
using Project21032025.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);
DotNetEnv.Env.Load();
var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings");
var jwt_Scret = Environment.GetEnvironmentVariable("API_Secret");

builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
{
    options.UseMySQL(connectionString);
});

builder.Services.AddScoped<ILeaveRepository, LeaveRepository>();
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
