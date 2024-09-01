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

// Configurar JWT
var jwtSettings = builder.Configuration.GetSection ( "JwtSettings" );
var secretKey = jwtSettings [ "SecretKey" ];

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
        IssuerSigningKey = new SymmetricSecurityKey ( Encoding.UTF8.GetBytes ( secretKey ) )
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
