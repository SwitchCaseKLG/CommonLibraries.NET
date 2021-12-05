using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SwitchCase.Web
{
    public abstract class FixedIntervalService : IHostedService, IDisposable
    {
        private readonly ILogger logger;
        private Timer? timer;
        public TimeSpan Interval { get; set; }

        public FixedIntervalService(ILogger logger)
        {
            this.logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation($"Background Worker Service running with Interval={Interval.TotalSeconds}s");
            timer = new Timer(ExecuteTaskAsync, null, TimeSpan.Zero, Interval);
            return Task.CompletedTask;
        }

        private async void ExecuteTaskAsync(object? state)
        {
            logger.LogTrace("Execute task");
            await RunJobAsync();
        }

        protected abstract Task RunJobAsync();

        public Task StopAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Background Worker Service is stopping.");
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
