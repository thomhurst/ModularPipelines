using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Engine;
using Vertical.SpectreLogger.Core;

namespace ModularPipelines.DependencyInjection;

[ExcludeFromCodeCoverage]
internal class SecretsLogFilter : ILogEventFilter
{
    internal ISecretProvider? SecretProvider { get; set; }

    public bool Filter(in LogEventContext eventContext)
    {
        if (SecretProvider is null)
        {
            return true;
        }
        
        var stringValue = eventContext.State?.ToString();
        
        if (string.IsNullOrWhiteSpace(stringValue))
        {
            return true;
        }

        return !SecretProvider.Secrets.Any(secret => stringValue.Contains(secret));
    }
}