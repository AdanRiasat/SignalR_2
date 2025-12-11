using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using signalr.backend.Data;
using signalr.backend.Hubs;
using signalr.backend.Models;


namespace signalr.backend.Services
{
    public class BGroundService : BackgroundService
    {
        public const int DELAY = 30 * 1000;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IHubContext<ChatHub> _hubContext;

        public BGroundService(IHubContext<ChatHub> hubContext, IServiceScopeFactory serviceScopeFactory)
        {
            _hubContext = hubContext;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task PopularChannel()
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var maxAmount = await dbContext.Channel.MaxAsync(c => c.NbMessages);
                Channel? MostPopularChannel = await dbContext.Channel.FirstOrDefaultAsync(c => c.NbMessages == maxAmount);

                if (MostPopularChannel == null) return;

                await _hubContext.Clients.Group("Channel" + MostPopularChannel.Id).SendAsync("MostPopularChannel", maxAmount);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await PopularChannel();
                await Task.Delay(DELAY, stoppingToken);
            }
        }
    }
}
