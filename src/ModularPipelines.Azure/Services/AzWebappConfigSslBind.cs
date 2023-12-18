using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "config", "ssl")]
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