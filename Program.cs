using ZeissTest.Hub;
using ZeissTest.Interface;
using ZeissTest.Repository;
using ZeissTest.Services;
using Serilog;
using ZeissTest.Extension;
using ZeissTest.Middleware;
using System.Diagnostics.Metrics;

var builder = WebApplication.CreateBuilder(args);

//build log
Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();
SerilogExtension.AddSerilogApi(builder.Configuration);
builder.Host.UseSerilog(Log.Logger);

ConfigureServices(builder.Services);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//metrics for collecting the data.
var meter = new Meter("Microsoft.AspNetCore", "v1.0");
Counter<int> counter = meter.CreateCounter<int>("Requests");
app.Use((context, next) =>
{
    counter.Add(1, KeyValuePair.Create<string, object?>("path", context.Request.Path.ToString()));
    return next(context);
});

app.UseSerilogRequestLogging();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    c.RoutePrefix = string.Empty;
});

ConfigureMiddleware(app, app.Services);
ConfigureEndpoints(app, app.Services);

app.Run();

void ConfigureServices(IServiceCollection services) {
    services.AddMvcCore().AddApiExplorer();
    services.AddSingleton<ICacheService, CacheService>();
    services.AddSingleton<IHub, MachineHub>();
    services.AddHostedService<DataHostServices>();
    services.AddMemoryCache();
    services.AddHealthChecks();
}

void ConfigureMiddleware(IApplicationBuilder appBuilder, IServiceProvider services) {
    appBuilder.UseHttpsRedirection();
    appBuilder.UseMiddleware<ErrorHandlingMiddleware>();
}

void ConfigureEndpoints(IEndpointRouteBuilder endpointRouteBuilder, IServiceProvider services) {
    endpointRouteBuilder.MapSwagger();
    endpointRouteBuilder.MapControllers();
    endpointRouteBuilder.MapHealthChecks("/healthz");
}
