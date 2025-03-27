using Hangfire;
using Hangfire.PostgreSql;
using HangfireBasicAuthenticationFilter;
using IService;
using Newtonsoft.Json;
using Npgsql;

namespace Exemple_Dotnet_API.Extensions
{
    public static class HangfireConfigurator
    {
        public static void ConfigureHangfireDashboardAndJobs(
            IApplicationBuilder app,
            IConfiguration configuration
        )
        {
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 1 });

            try
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var connectionStringStorage = configuration.GetSection("HangfireSettings")[
                        "ConnectionStringStorage"
                    ];
                    var builder = new NpgsqlConnectionStringBuilder(connectionStringStorage);

                    var storageOptions = new PostgreSqlStorageOptions
                    {
                        DistributedLockTimeout = TimeSpan.FromMinutes(1),
                        QueuePollInterval = TimeSpan.FromSeconds(15),
                        JobExpirationCheckInterval = TimeSpan.FromHours(1),
                        CountersAggregateInterval = TimeSpan.FromMinutes(5),
                        PrepareSchemaIfNecessary = false, // Set this to true to create the schema if necessary
                        //SchemaName = "public"
                    };

                    var username = configuration.GetSection("HangfireSettings")["HangfireUser"];
                    var password = configuration.GetSection("HangfireSettings")["hangfirePass"];

                    app.UseHangfireDashboard(
                        "/hangfire",
                        new DashboardOptions
                        {
                            DashboardTitle = "Hangfire Dashboard",
                            Authorization = new[]
                            {
                                new HangfireCustomBasicAuthenticationFilter
                                {
                                    User = username,
                                    Pass = password
                                }
                            }
                        }
                    );

                    GlobalConfiguration.Configuration.UsePostgreSqlStorage(
                        configure => configure.UseNpgsqlConnection(builder.ConnectionString),
                        storageOptions
                    );

                    GlobalConfiguration.Configuration.UseSerializerSettings(
                        new Newtonsoft.Json.JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                            TypeNameHandling = TypeNameHandling.All
                        }
                    );

                    AddOrUpdateJobs();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ConfigureHangfireDashboardAndJobs: {ex.Message}");
                if (
                    ex.Message
                    != "An exception has been raised that is likely due to a transient failure."
                )
                {
                    throw;
                }
            }
        }

        private static void AddOrUpdateJobs()
        {
            var recurringJobOptions = new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Local
                // TimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")
            };

            RecurringJob.AddOrUpdate<IHangfireJobService>(
                "Test_CreateJob",
                jobService => jobService.TestCreateJob(),
                "* * * * *",
                recurringJobOptions
            );

            RecurringJob.AddOrUpdate<IHangfireJobService>(
                "Test_CreateRecurringJob",
                jobService => jobService.TestCreateRecurringJob(),
                "*/3 * * * *",
                recurringJobOptions
            );

            // RecurringJob.AddOrUpdate<IHangfireJobService>(
            //     "RemoveOldLogs",
            //     jobService => jobService.RemoveOldLogs(),
            //     Cron.Monthly(day: 1, hour: 9), // รันทุกวันที่ 1 เวลา 9 โมงเช้า
            //     recurringJobOptions
            // );
        }
    }
}
