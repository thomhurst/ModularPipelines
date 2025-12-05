using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("webapp")]
public class AzWebappRestart
{
    public AzWebappRestart(
        AzWebappRestartAppserviceKube appserviceKube
    )
    {
        AppserviceKube = appserviceKube;
    }

    public AzWebappRestartAppserviceKube AppserviceKube { get; }
}