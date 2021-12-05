using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SwitchCase.Web
{
    public abstract class FixedDelayService : IHostedService, IDisposable
    {
        protected readonly ILogger logger;
        private Timer? _timer;
        private Task? _executingTask;
        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();

        IServiceProvider _services;
        public FixedDelayService(IServiceProvider services, ILogger logger)
        {
            _services = services;
            this.logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(ExecuteTask, null, FirstRunAfter, Timeout.InfiniteTimeSpan);
            logger.LogInformation("Start background task");
            return Task.CompletedTask;
        }

        private void ExecuteTask(object? state)
        {
            _timer?.Change(Timeout.Infinite, 0);
            _executingTask = ExecuteTaskAsync(_stoppingCts.Token);
        }

        private async Task ExecuteTaskAsync(CancellationToken stoppingToken)
        {
            logger.LogTrace("Execute task");
            try
            {
                await RunJobAsync(stoppingToken);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "BackgroundTask Failed");
            }
            _timer?.Change(Interval, Timeout.InfiniteTimeSpan);
        }

        protected abstract Task RunJobAsync(CancellationToken stoppingToken);

        protected TimeSpan Interval { get; set; }

        protected TimeSpan FirstRunAfter { get; set; }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            // Stop called without start
            if (_executingTask == null)
            {
                return;
            }

            try
            {
                // Signal cancellation to the executing method
                _stoppingCts.Cancel();
            }
            finally
            {
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }

        }

        public void Dispose()
        {
            _stoppingCts.Cancel();
            _timer?.Dispose();
        }
    }
}
