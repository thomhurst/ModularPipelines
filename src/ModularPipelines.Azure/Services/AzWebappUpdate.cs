using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("webapp")]
public class AzWebappUpdate
{
    public AzWebappUpdate(
        AzWebappUpdateAppserviceKube appserviceKube
    )
    {
        AppserviceKube = appserviceKube;
    }

    public AzWebappUpdateAppserviceKube AppserviceKube { get; }
}