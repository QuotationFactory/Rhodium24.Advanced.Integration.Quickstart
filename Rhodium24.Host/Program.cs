using System;
using System.Linq;
using MediatR;
using MetalHeaven.Integration.Shared.Classes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Rhodium24.Host
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs\\log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                Log.Information("Starting host");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions<AgentSettings>()
                        .Bind(hostContext.Configuration.GetSection("AgentSettings"))
                        .ValidateDataAnnotations();

                    services.AddHostedService<IntegrationWorker>();

                    services.AddMediatR(typeof(BaseIntegration).Assembly);
                    
                    RegisterIntegrations(services);
                });

        private static void RegisterIntegrations(IServiceCollection services)
        {
            var assemblies = IntegrationHelper.GetAssemblies().ToArray();

            if (!assemblies.Any()) return;

            services.AddMediatR(assemblies);

            var integrationServiceProvider = services.BuildServiceProvider();

            foreach (var integration in IntegrationHelper.GetClassesFromType<BaseIntegration>())
            {
                var instance = (BaseIntegration) ActivatorUtilities.CreateInstance(integrationServiceProvider, integration);

                services.AddSingleton(typeof(BaseIntegration), instance);

                Log.Information("Registering integration {name} ({id})", instance.Name, instance.Id);

                instance.RegisterServices(services);
            }
        }
    }
}