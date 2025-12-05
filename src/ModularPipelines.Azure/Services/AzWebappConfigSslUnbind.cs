using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("webapp", "config", "ssl")]
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