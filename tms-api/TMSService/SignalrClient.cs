using Data.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TMSService
{
    public class SignalrClient<T>
    {
        public HubConnection _client;
        private readonly ILogger<T> _logger;
        HubConnectionState state = HubConnectionState.Disconnected;
        public SignalrClient(ILogger<T> logger)
        {
            _logger = logger;
        }

        public async System.Threading.Tasks.Task InitialConnection()
        {
            _client = new HubConnectionBuilder()
          .WithUrl("http://10.4.4.224:1066/working-management-hub")
          .Build();
            ClosedHub();
            ReconnectedHub();
            ReconnectingHub();
            await ConnectWithRetryAsync();
        }
        private void ClosedHub()
        {
            _client.Closed += async (error) =>
            {
                //await System.Threading.Tasks.Task.Delay(new Random().Next(0, 5) * 1000);
                _logger.LogInformation($"Envent: Closed - The signalr client is restarting!");
                state = HubConnectionState.Disconnected;
                while (true)
                {
                    if (await ConnectWithRetryAsync())
                    {
                        break;
                    }
                }
            };
        }
        private async System.Threading.Tasks.Task AutoConnectingHub()
        {
            while (state == HubConnectionState.Disconnected)
            {
                await ConnectWithRetryAsync();
            }
        }
        private void ReconnectedHub()
        {
            _client.Reconnected += async (connectionId) =>
            {
                _logger.LogInformation($"Connection successfully reconnected. The ConnectionId is now: {connectionId}");
                state = HubConnectionState.Connected;

                while (true)
                {
                    if (await ConnectWithRetryAsync())
                    {
                        break;
                    }
                }
            };
        }
        private void ReconnectingHub()
        {
            _client.Reconnecting += async error =>
            {
                _logger.LogInformation($"Connection started reconnecting due to an error: {error.Message}");
                _logger.LogInformation($"State Hub {_client.State} - State Global {state}");
                if (_client.State == HubConnectionState.Reconnecting)
                    state = HubConnectionState.Disconnected;
                while (true)
                {
                    if (await ConnectWithRetryAsync())
                    {
                        break;
                    }
                }
            };
        }
        private async Task<bool> ConnectWithRetryAsync()
        {
            // Keep trying to until we can start or the token is canceled.
            while (true)
            {

                try
                {
                    await _client.StartAsync();
                    // Debug.Assert(_connection.State == HubConnectionState.Connected);
                    _logger.LogInformation($"The signalr client connected at {DateTime.Now.ToString()}");
                    state = HubConnectionState.Connected;
                    return true;
                }
                catch
                {
                    // Failed to connect, trying again in 5000 ms.
                    // Debug.Assert(_connection.State == HubConnectionState.Disconnected);
                    await System.Threading.Tasks.Task.Delay(3000);
                    _logger.LogInformation($"The signalr client is reconnecting at {DateTime.Now.ToString()}");

                }
                if (state == HubConnectionState.Connected)
                {
                    return true;
                }
            }
        }
    }
}
