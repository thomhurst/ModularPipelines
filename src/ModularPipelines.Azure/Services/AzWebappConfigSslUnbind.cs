using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "config", "ssl")]
public class AzWebappConfigSslUnbind
{
    public AzWebappConfigSslUnbind(
        AzWebappConfigSslUnbindAppserviceKube appserviceKube
    )
    {
        AppserviceKube = appserviceKube;
    }

    public AzWebappConfigSslUnbindAppserviceKube AppserviceKube { get; }
}