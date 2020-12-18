using System;
using System.Threading;
using System.Threading.Tasks;
using MetalHeaven.Integration.Shared.Classes;
using MetalHeaven.Integration.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Rhodium24.Integration
{
    public class ExampleIntegration : BaseIntegration
    {
        private readonly ILogger<ExampleIntegration> _logger;
        public override Guid Id { get; set; } = Guid.Parse("d30b30a4-66db-4bd3-b58e-1067424b1692");
        public override string Name { get; set; } = "Example integration";

        public ExampleIntegration(ILogger<ExampleIntegration> logger)
        {
            _logger = logger;
        }
        
        public override void RegisterServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddHostedService<IntegrationOutputFileWatcher>();
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting");
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping");
            return Task.CompletedTask;
        }
    }
}