using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using WebApplicationsgls.API.Authorization;
using WebApplicationsgls.Application.Interfaces;
using WebApplicationsgls.Application.Services;
using WebApplicationsgls.Infrastructure.Data;
using WebApplicationsgls.Infrastructure.Repositories;
using WebApplicationsgls.Infrastructure.Storage;
using WebApplicationsgls.API.RateLimiting;
using AspNetCoreRateLimit;
using WebApplicationsgls.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

#region Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
#endregion

#region DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);
#endregion

#region JWT
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.MapInboundClaims = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero,
        RoleClaimType = TokenClaims.Roles
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthorizationPolicies.InternalUser, policy =>
        policy.RequireAuthenticatedUser()
            .RequireClaim(TokenClaims.AccessType, TokenClaims.InternalUser));

    options.AddPolicy(AuthorizationPolicies.Client, policy =>
        policy.RequireAuthenticatedUser()
            .RequireClaim(TokenClaims.AccessType, TokenClaims.Client));

    options.AddPolicy(AuthorizationPolicies.Admin, policy =>
        policy.RequireAuthenticatedUser()
            .RequireClaim(TokenClaims.AccessType, TokenClaims.InternalUser)
            .RequireRole("Admin"));

    options.AddPolicy(AuthorizationPolicies.VendedorOuAdmin, policy =>
        policy.RequireAuthenticatedUser()
            .RequireClaim(TokenClaims.AccessType, TokenClaims.InternalUser)
            .RequireRole("Admin", "Vendedor"));

    options.AddPolicy(AuthorizationPolicies.LojaBasica, policy =>
        policy.RequireAuthenticatedUser()
            .RequireClaim(TokenClaims.AccessType, TokenClaims.InternalUser)
            .RequireRole("Admin", "Vendedor", "Marketing"));
});
#endregion

#region Dependency Injection
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IMovimentacaoEstoqueRepository, MovimentacaoEstoqueRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IRelatorioRepository, RelatorioRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();

builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<ProdutoService>();
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<EstoqueService>();
builder.Services.AddScoped<PedidoService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthClienteService>();
builder.Services.AddScoped<RelatorioService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IImageStorageService>(_ =>
{
    var uploadsFolderName = builder.Configuration["ImageStorage:UploadsFolder"] ?? "Uploads";
    var requestPath = builder.Configuration["ImageStorage:RequestPath"] ?? "/uploads";
    var uploadsPath = Path.Combine(builder.Environment.ContentRootPath, uploadsFolderName);

    return new LocalImageStorageService(uploadsPath, requestPath);
});
#endregion

#region CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
#endregion

#region Rate Limiting
builder.Services.AddApiRateLimiting(builder.Configuration);
#endregion

#region Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SGLS API",
        Version = "v1",
        Description = "API de gerenciamento com JWT"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Bearer {token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});
#endregion

#region App
var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

var uploadsFolderName = builder.Configuration["ImageStorage:UploadsFolder"] ?? "Uploads";
var uploadsRequestPath = builder.Configuration["ImageStorage:RequestPath"] ?? "/uploads";
var uploadsPath = Path.Combine(app.Environment.ContentRootPath, uploadsFolderName);
Directory.CreateDirectory(uploadsPath);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = uploadsRequestPath
});

app.UseIpRateLimiting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion
