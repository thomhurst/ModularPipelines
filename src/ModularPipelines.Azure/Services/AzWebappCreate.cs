using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("webapp")]
public class AzWebappCreate
{
    public AzWebappCreate(
        AzWebappCreateAppserviceKube appserviceKube
    )
    {
        AppserviceKube = appserviceKube;
    }

    public AzWebappCreateAppserviceKube AppserviceKube { get; }
}