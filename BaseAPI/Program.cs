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

//Primero carga appsettings.json, y después sobreescribe todo lo que venga en variables de entorno.

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();
//para que escuche en el puerto que le indique Railway
//busca la variable de entorno PORT de Railway y lo guarda en una bariable.
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

//sirve para exponer un endpoint que informe si la aplicación está "viva"
builder.Services.AddHealthChecks();

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

    //-- Agreba mas informacion al swagger --
    setupAction.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API de Gestión de Proyectos",
        Version = "v1",
        Description = "Esta API permite gestionar Tres tipos de usuarios (administrador, cliente y empleado), herramientas y proyectos. Algunos endpoints requiere autenticación por JWT. Recuperacion de contraseña por email. ",
        Contact = new OpenApiContact
        {
            Name = "Nicolas Romero Barrios",
            Email = "nicolasromero.barrios@gmail.com",
            Url = new Uri("https://github.com/N-Rem"),
        }
    });
});

var secretKey = builder.Configuration["AuthenticationService:SecretForKey"];
if (string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException("JWT_SECRET is not defined either as an environment variable or in appsettings.json. Contiene recuperacion de contraseña por mail.");
}
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey))
        };
    });


//Define políticas de autorización para usar en los controladores
//para restringir el acceso según el rol del usuario autenticado.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ClientEmployeeAdmin", policy =>
        policy.RequireRole("Client", "Employee", "Admin"));
});

#endregion

#region MySql se cambia a posgreMsQL para poder deployar
//string connectionString = builder.Configuration.GetConnectionString("DBConnectionString")!;
string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
    ?? builder.Configuration.GetConnectionString("DBConnectionString")!;
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("No se encontró la cadena de conexión a la base de datos. Asegúrate de que la variable de entorno 'DB_CONNECTION_STRING' esté configurada correctamente.");
}

builder.Services.AddDbContext<AppDbContext>(opcion =>
{
    opcion.UseNpgsql(connectionString,
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
//if (app.Environment.IsDevelopment())
//{
//}
app.UseStaticFiles(); // <- Esto habilita los archivos desde wwwroot
app.UseSwagger();
app.UseHealthChecks("/health");
app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Gestión v1");
        c.InjectStylesheet("/swagger-ui/custom.css"); // agregás tu CSS //!!!!!!!!!!!!!!!!!!
    });

app.UseHttpsRedirection();

app.UseAuthentication(); //fundamental para usar el JWT

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        db.Database.Migrate(); // Ejecuta las migraciones pendientes
    }
    catch (Exception ex)
    {
        // Registra el error y detiene la aplicación
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error al aplicar las migraciones.");
        throw;
    }
}


app.Run();
