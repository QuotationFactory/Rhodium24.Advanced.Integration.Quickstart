using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MetalHeaven.Integration.Shared.Classes;
using Microsoft.Extensions.Hosting;

namespace Rhodium24.Host
{
    public class IntegrationWorker : IHostedService
    {
        private readonly IEnumerable<BaseIntegration> _integrations;

        public IntegrationWorker(IEnumerable<BaseIntegration> integrations)
        {
            _integrations = integrations;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var startTasks = _integrations.Select(x => x.StartAsync(cancellationToken));
            await Task.WhenAll(startTasks).ConfigureAwait(false);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            var startTasks = _integrations.Select(x => x.StartAsync(cancellationToken));
            await Task.WhenAll(startTasks).ConfigureAwait(false);
        }
    }
}