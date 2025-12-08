using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("webapp")]
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