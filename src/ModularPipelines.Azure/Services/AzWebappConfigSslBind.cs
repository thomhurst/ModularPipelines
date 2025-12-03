using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("webapp", "config", "ssl")]
public class AzWebappConfigSslBind
{
    public AzWebappConfigSslBind(
        AzWebappConfigSslBindAppserviceKube appserviceKube
    )
    {
        AppserviceKube = appserviceKube;
    }

    public AzWebappConfigSslBindAppserviceKube AppserviceKube { get; }
}