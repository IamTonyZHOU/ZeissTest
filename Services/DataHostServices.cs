using ZeissTest.Interface;

namespace ZeissTest.Services;

public class DataHostServices: IHostedService
{
    private readonly IHub _hub;
    
    public DataHostServices(IHub hub)
    {
        _hub = hub;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _hub.GetMachineInfoFromWS();
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}