using Alianza.Models;
using Alianza.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder ( args );


// Configurar el cargador de configuración para soportar múltiples entornos
builder.Configuration
    .SetBasePath ( Directory.GetCurrentDirectory ( ) )
    .AddJsonFile ( "appsettings.json" , optional: false , reloadOnChange: true )
    .AddJsonFile ( $"appsettings.{builder.Environment.EnvironmentName}.json" , optional: true ) // Cargar configuraciones específicas del entorno
    .AddEnvironmentVariables ( );

// Add services to the container.
builder.Services.AddControllers ( );

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer ( );
builder.Services.AddSwaggerGen ( );

var ConnectionString = builder.Configuration.GetConnectionString ( "alianza_db" );
builder.Services.AddDbContext<AlianzaContext> ( options => options.UseSqlServer ( ConnectionString ) );

// Registrar el servicio TokenService
builder.Services.AddScoped<TokenService> ( ); // <--- Registrar el servicio aquí

// Configurar CORS para permitir solicitudes de cualquier origen
builder.Services.AddCors ( options =>
{
    options.AddPolicy ( "AllowAllOrigins" ,
        builder => builder.AllowAnyOrigin ( )
                          .AllowAnyMethod ( )
                          .AllowAnyHeader ( ) );
} );

// Configurar JWT
var jwtSettings = builder.Configuration.GetSection ( "JwtSettings" );
var secretKeys = jwtSettings [ "SecretKey" ];

builder.Services.AddAuthentication ( options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
} )
.AddJwtBearer ( options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true ,
        ValidateAudience = true ,
        ValidateLifetime = true ,
        ValidateIssuerSigningKey = true ,
        ValidIssuer = jwtSettings [ "Issuer" ] , // URL local
        ValidAudience = jwtSettings [ "Audience" ] , // URL local
        IssuerSigningKey = new SymmetricSecurityKey ( Encoding.UTF8.GetBytes ( secretKeys ) )
    };

    // Configurar manejo de errores de autenticación
    options.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {
            // Evitar la respuesta predeterminada para un error 401
            context.HandleResponse ( );

            // Crear el objeto ResponseData con el mensaje de error
            var response = new ResponseData
            {
                Data = null ,
                Message = "No autorizado: Token no válido o faltante." ,
                Status = 401
            };

            // Configurar el código de estado y devolver la respuesta en formato JSON
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 401; // Código de estado HTTP 401 (Unauthorized)
            await context.Response.WriteAsJsonAsync ( response );
        }
    };
} );

builder.Services.AddSwaggerGen ( c =>
{
    c.AddSecurityDefinition ( "Bearer" , new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header ,
        Description = "Por favor, ingrese 'Bearer' seguido de un espacio y su token JWT" ,
        Name = "Authorization" ,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    } );

    c.AddSecurityRequirement ( new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    } );
} );

var app = builder.Build ( );

// Configurar Middleware
app.UseMiddleware<ErrorHandlingMiddleware> ( ); // Usar el middleware de manejo de errores


// Usar la política de CORS configurada
app.UseCors ( "AllowAllOrigins" );

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment ( ))
{
    app.UseSwagger ( );
    app.UseSwaggerUI ( );
}

app.UseHttpsRedirection ( );

app.UseAuthentication ( ); // Habilitar autenticación
app.UseAuthorization ( );

app.MapControllers ( );

app.Run ( );
