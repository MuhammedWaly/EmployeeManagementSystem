using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Helpers;
using ServerLibrary.Reposaitories.Implementions;
using ServerLibrary.Reposaitories.Contracts;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BaseLibrary.Entities;
using Server.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefultConnections") ?? throw new InvalidOperationException("No ConnectionString"));

});

builder.Services.Configure<Jwt>(builder.Configuration.GetSection("Jwt"));
var JwtSection = builder.Configuration.GetSection(nameof(Jwt)).Get<Jwt>();

builder.Services.AddScoped<IUserAccount, UserAccount>();

builder.Services.AddAuthentication(op =>
{
    op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(op =>
{
    op.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = JwtSection!.Audience,
        ValidIssuer = JwtSection!.Issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSection.Key!))
    };
});

builder.Services.AddScoped<IGenericReposaitory<GeneralDepartment>, GeneralDepartmentReposaitory>();
builder.Services.AddScoped<IGenericReposaitory<Town>, TownReposaitory>();
builder.Services.AddScoped<IGenericReposaitory<Country>, CountryReposaitory>();
builder.Services.AddScoped<IGenericReposaitory<Department>, DepartmentReposaitory>();
builder.Services.AddScoped<IGenericReposaitory<City>, CityReposaitory>();
builder.Services.AddScoped<IGenericReposaitory<Branch>, BranchReposaitory>();
builder.Services.AddScoped<IGenericReposaitory<Employee>, EmployeeReposaitory>();
builder.Services.AddScoped<IGenericReposaitory<Doctor>, DoctorReposaitory>();
builder.Services.AddScoped<IGenericReposaitory<Vacation>, VacationReposaitory>();
builder.Services.AddScoped<IGenericReposaitory<VacationType>, VacationTypeReposaitory>();
builder.Services.AddScoped<IGenericReposaitory<Sanction>, SanctionRepsaitory>();
builder.Services.AddScoped<IGenericReposaitory<SanctionType>, SancationTypeReposaitory>();
builder.Services.AddScoped<IGenericReposaitory<Overtime>, OvertimeReposaitory>();
builder.Services.AddScoped<IGenericReposaitory<OvertimeType>, OvertimeTypeReposaitory>();

builder.Services.AddCors(op =>
{
    op.AddPolicy("BlazorWasm", builder =>
    builder.WithOrigins("https://localhost:7271")
    .AllowCredentials()
    .AllowAnyHeader()
    .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("BlazorWasm");

app.UseAuthentication();

app.UseAuthorization();

app.UseRouting();

app.MapControllers();

app.Run();
