using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("functionapp", "deployment", "source")]
public class AzFunctionappDeploymentSourceConfigZip
{
    public AzFunctionappDeploymentSourceConfigZip(
        AzFunctionappDeploymentSourceConfigZipAppserviceKube appserviceKube
    )
    {
        AppserviceKube = appserviceKube;
    }

    public AzFunctionappDeploymentSourceConfigZipAppserviceKube AppserviceKube { get; }
}