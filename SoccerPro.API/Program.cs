using Azure.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using SoccerPro.API.Controllers.settings;
using SoccerPro.Application.Features.PlayerFeature.Commands.AddPlayer;
using SoccerPro.Domain.Entities;
using SoccerPro.Infrastructure;
using SoccerPro.Infrastructure.Data;
using SoccerPro.Infrastructure.Repository;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Data;


var builder = WebApplication.CreateBuilder(args);

// ✅ Use Azure-assigned port for Linux container
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

Console.WriteLine("🟢 PORT = " + Environment.GetEnvironmentVariable("PORT"));


// Enable controllers and JSON enum serialization
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
});




// 🔐 Load from Azure Key Vault using managed identity
string keyVaultName = "dbs123";
string dbSecretName = "myDB";
var kvUri = new Uri($"https://{keyVaultName}.vault.azure.net");
builder.Configuration.AddAzureKeyVault(kvUri, new DefaultAzureCredential());

// ✅ Get the connection string and save it under "ConnectionStrings:DefaultConnection"
var connStr = builder.Configuration[dbSecretName];

if (string.IsNullOrWhiteSpace(connStr))
    throw new InvalidOperationException("❌ Connection string not found in Azure Key Vault.");

builder.Configuration["ConnectionStrings:DefaultConnection"] = connStr;



string JWTSecretName = "kfupm-jwt";

// ✅ Get the JWT Key and save it under "JwtSettings:SecretKey"
var JWtKey = builder.Configuration[JWTSecretName];

if (string.IsNullOrWhiteSpace(JWtKey))
    throw new InvalidOperationException("❌ JWT Key not found in Azure Key Vault.");


builder.Configuration["JwtSettings:SecretKey"] = JWtKey;



// ✅ Provide ADO.NET connection (SqlConnection)
builder.Services.AddScoped<IDbConnection>(sp =>
{
    var cs = sp.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection");
    return new SqlConnection(cs);
});

// ✅ Provide EF Core Identity using the same connection
builder.Services.AddDbContext<AppDbContext>((sp, options) =>
{
    var cs = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(cs);
});

// ✅ ASP.NET Identity setup
builder.Services.AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


// Add JWT Config
// ADD JWT Config
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));




// Application & Infrastructure dependency injection
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfra();



// FluentValidation setup
builder.Services.AddValidatorsFromAssemblyContaining<AddPlayerCommandValidator>();

// Swagger setup
builder.Services.AddSingleton<ISchemaFilter, FluentValidationSchemaFilter>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = "SoccerPro.API.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    options.EnableAnnotations();
    options.SchemaFilter<EnumSchemaFilter>();
    options.SupportNonNullableReferenceTypes();
    options.UseAllOfToExtendReferenceSchemas();
    options.SchemaFilter<FluentValidationSchemaFilter>();

    // Add JWT auth to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token.\nExample: Bearer abc123xyz"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


var app = builder.Build();

app.UseMiddleware<GlobalExeptionHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();