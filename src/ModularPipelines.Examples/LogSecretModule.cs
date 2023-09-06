using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples;

public class LogSecretModule : Module
{
    private readonly IOptions<MyOptions> _options;
    private readonly ILogger<LogSecretModule> _logger;

    public LogSecretModule(IOptions<MyOptions> options, ILogger<LogSecretModule> logger)
    {
        _options = options;
        _logger = logger;
    }
    
    protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        context.Logger.LogInformation("Value is {Value}", _options.Value.MySecret);
        await Task.Yield();
        return null;
    }
}