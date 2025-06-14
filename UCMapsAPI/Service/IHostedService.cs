public class AppStartupHostedService : IHostedService
{
    private readonly ILogger<AppStartupHostedService> _logger;
    private readonly IConfiguration _configuration;

    public AppStartupHostedService(ILogger<AppStartupHostedService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        _logger.LogInformation($"Connection String: {connectionString}");

        // Perform any additional startup logic here...

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
