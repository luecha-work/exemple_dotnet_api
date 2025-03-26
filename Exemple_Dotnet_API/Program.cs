using AspNetCoreRateLimit;
using Entities;
using Hangfire;
using HealthChecks.UI.Client;
using IRepository;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Exemple_Dotnet_API.Extensions;
using Exemple_Dotnet_API.HealthCheck;
using Exemple_Dotnet_API.Middleware;
using Serilog;
using Shared.ConfigurationModels;
using Exemple_Dotnet_API.ContextFactory;
using IService;
using Service;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ExempleApiDbContext>(
    options =>
    {
        options.UseNpgsql(connectionString);
    },
    ServiceLifetime.Scoped,
    ServiceLifetime.Singleton
);

builder.Host.ConfigureSerilog();
builder.Services.ConfigureCors(builder.Configuration);
builder.Services.ConfigureIISIntegration();
builder.Services.AddMemoryCache();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureSession(builder.Configuration);
builder.Services.ConfigureRateLimitingOptions();
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureVersioning();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.ConfigureIdentityCore(builder.Configuration);
builder.Services.ConfigureHangfireJob(builder.Configuration);
builder.Services.AddAuthentication();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IUserProvider, UserProvider>();
builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.AddAuthProviderConfiguration(builder.Configuration);
builder.Services.AddAesEncryptionConfiguration(builder.Configuration);
AppContext.SetSwitch("System.Drawing.EnableUnixSupport", true);

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

//TODO: Add Scoped
builder.Services.AddScoped(
    typeof(IGenericRepositoryEntityFramework<>),
    typeof(Repository.EntityFramework.GenericRepository<>)
);
builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.AddScoped<Repository.Dapper.RepositoryManager>();
builder.Services.AddScoped<Repository.EntityFramework.RepositoryManager>();
builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();

//TODO: Add HangfireJob Service
builder.Services.AddScoped<IService.IHangfireJobService, Service.HangfireJobService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder
    .Services.AddHealthChecks()
    .AddCheck<ServiceHealthCheck>(nameof(ServiceHealthCheck))
    .AddCheck<DbHealthCheck>(nameof(DbHealthCheck))
    .AddCheck<ApiHealthCheck>(nameof(ApiHealthCheck));

builder
    .Services.AddControllers(options =>
    {
        options.Filters.Add(new ValidateModelStateFilter());
    });

builder.Services.AddHealthChecksUI().AddInMemoryStorage();

var app = builder.Build();

app.ConfigureExceptionHandler();

if (app.Environment.IsProduction())
{
    app.UseHsts();
    app.Use(
        async (context, next) =>
        {
            context.Response.Headers.Append("Content-Security-Policy", "default-src 'self';");

            await next();
        }
    );
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Request size limits are applied at the application level.
app.Use(
    async (context, next) =>
    {
        var maxRequestBodySizeFeature = context.Features.Get<IHttpMaxRequestBodySizeFeature>();
        if (maxRequestBodySizeFeature != null)
        {
            maxRequestBodySizeFeature.MaxRequestBodySize = 150 * 1024 * 1024; // 150 MB
        }
        await next();
    }
);

app.Use(
    async (context, next) =>
    {
        // Set X-Frame-Options header to prevent web pages from being embedded in iframes from different sources
        context.Response.Headers.Append("X-Frame-Options", "SAMEORIGIN");

        // Enable XSS (Cross-Site Scripting) protection by setting the X-XSS-Protection header
        context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");

        // Set X-Content-Type-Options header to prevent browsers from interpreting files as something else than declared in the content
        context.Response.Headers.Append("X-Content-Type-Options", "nosniff");

        // Set Referrer-Policy header to specify how much referrer information to include with requests
        context.Response.Headers.Append("Referrer-Policy", "strict-origin");

        // Call the next middleware in the pipeline
        await next();
    }
);

HangfireConfigurator.ConfigureHangfireDashboardAndJobs(app, builder.Configuration);

app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.All });
app.UseSession();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseHangfireDashboard();
app.UseIpRateLimiting();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<SurveySystemSessionMiddleware>();
app.UseStaticFiles();

app.MapHealthChecks(
    "/healthcheck",
    new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    }
);

app.MapControllers();

app.MapHealthChecksUI();

app.Run();