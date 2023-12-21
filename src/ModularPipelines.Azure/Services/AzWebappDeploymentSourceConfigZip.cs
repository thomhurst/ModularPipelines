using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "deployment", "source")]
public class AzWebappDeploymentSourceConfigZip
{
    public AzWebappDeploymentSourceConfigZip(
        AzWebappDeploymentSourceConfigZipAppserviceKube appserviceKube
    )
    {
        AppserviceKube = appserviceKube;
    }

    public AzWebappDeploymentSourceConfigZipAppserviceKube AppserviceKube { get; }
}