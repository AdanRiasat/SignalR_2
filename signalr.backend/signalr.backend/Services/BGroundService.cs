using Microsoft.AspNetCore.SignalR;

namespace signalr.backend.Services
{
    public class BGroundService : BackgroundService
    {
        public const int DELAY = 30 * 1000;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IHubContext _hubContext;

        public BGroundService(IHubContext hubContext, IServiceScopeFactory serviceScopeFactory)
        {
            _hubContext = hubContext;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(DELAY, stoppingToken);
            }
        }
    }
}
