using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Veterinaria.Domain.Entities.ApplicationUser;
using Veterinaria.Domain.Entities.Invoices;
using Veterinaria.Domain.Entities.MedicalConsultations;
using Veterinaria.Domain.Entities.Owners;
using Veterinaria.Domain.Entities.PetOwners;
using Veterinaria.Domain.Entities.Pets;
using Veterinaria.Domain.Entities.Sécialities;
using Veterinaria.Domain.Entities.Users;
using Veterinaria.Domain.Entities.Vets;
using Veterinarian.Api.ExtensionsFiled;
using Veterinarian.Application;
using Veterinarian.Application.AuthServices;
using Veterinarian.Application.Invoices;
using Veterinarian.Application.MedicalConsultations;
using Veterinarian.Application.Owners;
using Veterinarian.Application.Pets;
using Veterinarian.Application.Repositories;
using Veterinarian.Application.Specialities;
using Veterinarian.Application.UnitsOfWork;
using Veterinarian.Application.Users;
using Veterinarian.Application.Vets;
using Veterinarian.Infrastructure;
using Veterinarian.Infrastructure.Configurations;
using Veterinarian.Infrastructure.Repositories;
using Veterinarian.Infrastructure.ServicesFiles;
using Veterinarian.Infrastructure.UnitsOfWork;
using Veterinarian.Security.SettingsFolder;
using Veterinarian.Security.Token;

var builder = WebApplication.CreateBuilder(args);


//Db Context
var connectionString = builder.Configuration.GetConnectionString("VeterinarianDB");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        connectionString,
        sqloptions => sqloptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Application));
});

// Security Context
var identityConnectionString = builder.Configuration.GetConnectionString("VeterinarianDB");
builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
{
    options.UseSqlServer(
        identityConnectionString,
        sqloptions => sqloptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName,Schemas.Identity));
    
});
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationIdentityDbContext>();

// Configuración del JWT
builder.Services.Configure<JwtAuthOptions>(builder.Configuration.GetSection("Jwt"));

//Convertimos en objeto
JwtAuthOptions jwtAutOptions = builder.Configuration.GetSection("Jwt").Get<JwtAuthOptions>()!;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = jwtAutOptions.Issuer,
        ValidAudience = jwtAutOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAutOptions.Key))
    };
});

builder.Services.AddAuthorization();

//Validator
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

//Services
builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<TokenProvider>();

builder.Services.AddScoped<IInvoicesServices, InvoicesServices>();
builder.Services.AddScoped<IMedicalConsultationServices, MedicalConsultationServices>();
builder.Services.AddScoped<IOwnerServices, OwnerServices>();
builder.Services.AddScoped<IPetServices, PetServices>();
builder.Services.AddScoped<ISpecialitiesServices, SpecialitiesServices>();
builder.Services.AddScoped<IVetServices, VetServices>();

builder.Services.AddScoped<IApplicationUserServices, ApplicationUserServices>();
builder.Services.AddScoped<IUserManagerServices, UserManagerServices>();

builder.Services.AddScoped<IInvoicesRepository, InvoicesRepository>();
builder.Services.AddScoped<IMedicalConsultationRepository, MedicalConsultationRepository>();
builder.Services.AddScoped<IOwnerRepositoy, OwnersRepository>();
builder.Services.AddScoped<IPetOwnerRepository, PetOwnerRepository>();
builder.Services.AddScoped<IPetsRepository, PetsRepository>();
builder.Services.AddScoped<ISpecialityRepository, SpecialitiesRepository>();
builder.Services.AddScoped<IVetsRepository, VetRepository>();

builder.Services.AddScoped<IApplicationUserRepository, ApplcationUserRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<InvoiceUnitOfWork>();
builder.Services.AddScoped<MedicalConsultationUnitOfWork>();
builder.Services.AddScoped<OwnerUnitOfWork>();
builder.Services.AddScoped<PetsUnitOfWork>();
builder.Services.AddScoped<SpecialitiesUnitOfWork>();
builder.Services.AddScoped<VetsUnitOfWork>();

builder.Services.AddScoped<UsertIdentityUnitOfWork>();

builder.Services.AddScoped<IUserContext, UserContext>();

builder.Services.AddMemoryCache();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API v1");
    });

    //await app.SeedInitialDataAsync();

}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
