using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

namespace ZeissTest.Extension;

public static class SerilogExtension
{
    public static void AddSerilogApi(IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
            .Enrich.WithExceptionDetails()
            .Enrich.WithCorrelationId()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("ApplicationName", $"Serilog - {Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}")
            .WriteTo.File("log.txt", outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }
}