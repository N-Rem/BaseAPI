using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services.AuthenticationServices;
using Infrastructure.Services.EmailServises;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


#region Edit JWT

//Edita el Swagger
builder.Services.AddSwaggerGen(setupAction =>
{
    //Nos permite el uso del jwt en el swagger
    setupAction.AddSecurityDefinition("API-BaseAuth", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Pegar el Token generado"
    });

    setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "API-BaseAuth"
                }
            }, new List<string>() 
        }
    });
});


//configuración del sistema de autenticación JWT. Le digo que voy a usar tokens JWT, y como se deben validar
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["AuthenticationService:Issuer"],
            ValidAudience = builder.Configuration["AuthenticationService:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["AuthenticationService:SecretForKey"]))
        };
    });

#endregion

#region MySql
string connectionString = builder.Configuration.GetConnectionString("DBConnectionString")!;

builder.Services.AddDbContext<AppDbContext>(opcion =>
{
    opcion.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
        b => b.MigrationsAssembly("Infrastructure"));
    opcion.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.AmbientTransactionWarning));
});

#endregion

#region Respositories

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IToolRepository, ToolRepository>();
builder.Services.AddScoped<IUserProjectRepository, UserProjectRepository>();


#endregion

#region Services
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IProjectServices, ProjectServices>();
builder.Services.AddScoped<IUserProjectServices, UserProjectServices>();
builder.Services.AddScoped<IToolServices, ToolServices>();

//JWT
builder.Services.Configure<AuthenticationServiceOptions>(
    builder.Configuration.GetSection(AuthenticationServiceOptions.AuthenticationService));
builder.Services.AddScoped<IAuthenticationService, AuthenticationServices>();
//Envio de emails
builder.Services.Configure<EmailSettingsOptions>(
    builder.Configuration.GetSection(EmailSettingsOptions.EmailService));
builder.Services.AddScoped<IEmailServices, EmailServices>();
#endregion




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); //fundamental para usar el JWT

app.UseAuthorization();

app.MapControllers();

app.Run();
