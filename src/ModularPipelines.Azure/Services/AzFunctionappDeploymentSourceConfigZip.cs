using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "deployment", "source")]
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