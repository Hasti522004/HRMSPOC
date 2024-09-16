using HRMSPOC.API.Data;
using HRMSPOC.API.Repositories;
using HRMSPOC.API.Repositories.Interfaces;
using HRMSPOC.API.Services;
using HRMSPOC.API.Services.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DbContext
builder.Services.AddDbContext<HRMSDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("conn")));

// Register Repository
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();

// Register Services
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();

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
