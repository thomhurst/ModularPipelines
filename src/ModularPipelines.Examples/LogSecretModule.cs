using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples;

public class LogSecretModule : IModule<IDictionary<string, object>?>
{
    private readonly IOptions<MyOptions> _options;

    public LogSecretModule(IOptions<MyOptions> options)
    {
        _options = options;
    }

    /// <inheritdoc/>
    public async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        context.Logger.LogInformation("Value is {Value}", _options.Value.MySecret);
        await Task.Yield();
        return null;
    }
}