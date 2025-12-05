using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("webapp")]
public class AzWebappShow
{
    public AzWebappShow(
        AzWebappShowAppserviceKube appserviceKube
    )
    {
        AppserviceKube = appserviceKube;
    }

    public AzWebappShowAppserviceKube AppserviceKube { get; }
}