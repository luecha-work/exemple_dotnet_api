using AspNetCoreRateLimit;
using Entities;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Npgsql;
using Serilog;
using Shared.ConfigurationModels;
using System.Text;

namespace Exemple_Dotnet_API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureSerilog(this IHostBuilder builder) =>
            builder
                .UseSerilog(
                    (context, loggerConfiguration) =>
                    {
                        loggerConfiguration
                            .WriteTo.Console()
                            .ReadFrom.Configuration(context.Configuration);
                    }
                );
        //.ConfigureServices(
        //    (context, services) =>
        //    {
        //        services.AddSingleton<ISerilogManagerFactory, SerilogManagerFactory>();
        //    }
        //);

        public static void ConfigureCors(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddCors(options =>
            {
                string allowedOrigins = configuration
                                    .GetSection("CorsPolicySettings")["AllowedOrigins"] ?? "*";
                var allowOrigin = allowedOrigins
                    .Split(",", StringSplitOptions.RemoveEmptyEntries);

                options.AddPolicy(
                    "CorsPolicy",
                    builder =>
                    {
                        builder
                            .WithOrigins(allowOrigin)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials()
                            .WithExposedHeaders("X-Pagination", "Location");
                    }
                );
            });
        }

        public static void ConfigureSession(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                //options.IdleTimeout = TimeSpan.FromDays(1);
                options.IdleTimeout = TimeSpan.FromMinutes(15);
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = "Exemple_Dotnet_API.AspNetCore.Session";
            });
        }

        public static void ConfigureRateLimitingOptions(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint = "*",
                    Limit = 1000,
                    Period = "1m"
                }
            };
            services.Configure<IpRateLimitOptions>(opt =>
            {
                opt.GeneralRules = rateLimitRules;
            });

            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        }

        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });
        }

        public static void ConfigureIISIntegration(this IServiceCollection services) =>
            services.Configure<IISOptions>(options => { });

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "ExempleDotnetBackend API",
                        Version = "v1", // Specify a valid version here
                        Description = "ExempleDotnet API by Luecha Kanmaneekul",
                    }
                );
                options.AddSecurityDefinition(
                    JwtBearerDefaults.AuthenticationScheme,
                    new OpenApiSecurityScheme
                    {
                        Description =
                            @"JWT Authorization header using the Bearer scheme.
                    Enter 'Bearer' [space] and then your token in the text input below.
                    Example: 'Bearer 12345abcdef'",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = JwtBearerDefaults.AuthenticationScheme
                    }
                );

                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        },
                        Scheme = "oauth2",
                        Name = JwtBearerDefaults.AuthenticationScheme,
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
                    }
                );
            });
        }


        public static void ConfigureJWT(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var jwtConfiguration = new JwtConfiguration();
            configuration.Bind(jwtConfiguration.Section, jwtConfiguration);
            var secret = jwtConfiguration.SecretKey;

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = jwtConfiguration.ValidIssuer,
                        ValidAudience = jwtConfiguration.ValidAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
                    };
                });
        }
        public static void ConfigureIdentityCore(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var identityConfigure = new IdentityProviderConfigure();
            configuration.Bind(identityConfigure.Section, identityConfigure);

            services
                .AddIdentityCore<Account>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                })
                .AddRoles<Role>()
                .AddTokenProvider<DataProtectorTokenProvider<Account>>(
                    identityConfigure.LoginProvider
                )
                .AddEntityFrameworkStores<ExempleApiDbContext>()
                .AddDefaultTokenProviders();

            //TODO: Config Limit Refresh Token Time
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(24);
                // options.TokenLifespan = TimeSpan.FromMinutes(5);
            });
        }

        public static void ConfigureHangfireJob(
            this IServiceCollection services,
            IConfiguration configuration
            )
        {
            var connectionStringStorage = configuration.GetSection("HangfireSettings")["ConnectionStringStorage"];

            var builder = new NpgsqlConnectionStringBuilder(connectionStringStorage);

            var storageOptions = new PostgreSqlStorageOptions
            {
                DistributedLockTimeout = TimeSpan.FromMinutes(1),
                QueuePollInterval = TimeSpan.FromSeconds(15),
                JobExpirationCheckInterval = TimeSpan.FromHours(1),
                CountersAggregateInterval = TimeSpan.FromMinutes(5),
                UseNativeDatabaseTransactions = true,
                PrepareSchemaIfNecessary = true,
            };

            services.AddHangfire(options =>
            {
                options.UsePostgreSqlStorage(
                    configure => configure.UseNpgsqlConnection(builder.ConnectionString),
                    storageOptions
                );
                options.UseSerializerSettings(
                    new Newtonsoft.Json.JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }
                );
            });

            services.AddHangfireServer();
        }

        public static void AddJwtConfiguration(
            this IServiceCollection services,
            IConfiguration configuration
        ) =>
            services.Configure<JwtConfiguration>(
                configuration.GetSection("JwtSettings")
            );

        public static void AddAuthProviderConfiguration(
            this IServiceCollection services,
            IConfiguration configuration
        ) =>
            services.Configure<IdentityProviderConfigure>(
                configuration.GetSection("AuthProvider")
            );

        public static void AddAesEncryptionConfiguration(
            this IServiceCollection services,
            IConfiguration configuration
        ) =>
            services.Configure<AesEncryptionConfigure>(
                configuration.GetSection("AesEncryptionSettings")
            );
    }
}
