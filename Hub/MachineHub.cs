
using System.Text.Json;
using ZeissTest.Models;
using Microsoft.AspNetCore.SignalR.Client;
using ZeissTest.Interface;
using Websocket.Client;

namespace ZeissTest.Hub;

public class MachineHub:Microsoft.AspNetCore.SignalR.Hub,IHub
{
    private readonly string _wsUrl;
    private readonly ICacheService _cacheService;
    private readonly ILogger<MachineHub> _logger;

    public MachineHub(IConfiguration configuration, ICacheService cacheService, ILogger<MachineHub> logger)
    {
        _wsUrl = configuration.GetSection("WebSocketUrl").Value;
        _cacheService = cacheService;
        _logger = logger;
    }

    private void SetCache(string msg, CancellationToken cancellationToken)
    {
        try
        {
            var machineInfo = JsonSerializer.Deserialize<MachineInfo>(msg);
            
            if (machineInfo != null)
            {
                _cacheService.SetAsync(Consts.MachineInfoCacheKey, machineInfo);
            }
            else
            {
                _logger.LogWarning($"Deserialized object is null, the message is {msg}.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during set cache.");
        }
    }

    public async Task GetMachineInfoFromWS()
    {
        try
        {
            WebsocketClient wsClient = new WebsocketClient(new Uri(_wsUrl));
            wsClient
                .MessageReceived
                .Subscribe( resp => { SetCache(resp.Text, CancellationToken.None); });
            await wsClient.Start();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during get machine info from web socket.");
        }
    }
}