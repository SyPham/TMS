using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TMSService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private  SignalrClient<Worker> _client;
        HubConnectionState state = HubConnectionState.Disconnected;

        public Worker(ILogger<Worker> logger)
        {
          
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
        public async override Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");
             _client = new SignalrClient<Worker>(_logger);
            await _client.InitialConnection();

        }

        private void DoWork(object state)
        {
          
            _logger.LogInformation(
                "Timed Hosted Service is working. Count: ");
        }

        public override Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");


            return Task.CompletedTask;
        }
       
    }
}
