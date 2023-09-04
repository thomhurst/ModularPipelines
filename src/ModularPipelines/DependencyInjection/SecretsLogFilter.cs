using ModularPipelines.Engine;
using Vertical.SpectreLogger.Core;

namespace ModularPipelines.DependencyInjection;

internal class SecretsLogFilter : ILogEventFilter
{
    private readonly ISecretProvider _secretProvider;

    public SecretsLogFilter(ISecretProvider secretProvider)
    {
        _secretProvider = secretProvider;
    }
    
    public bool Filter(in LogEventContext eventContext)
    {
        var stringValue = eventContext.State?.ToString();
        
        if (string.IsNullOrWhiteSpace(stringValue))
        {
            return true;
        }

        return !_secretProvider.Secrets.Any(secret => stringValue.Contains(secret));
    }
}