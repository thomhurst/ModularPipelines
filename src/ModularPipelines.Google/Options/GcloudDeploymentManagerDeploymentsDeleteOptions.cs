using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deployment-manager", "deployments", "delete")]
public record GcloudDeploymentManagerDeploymentsDeleteOptions(
[property: CliArgument] string DeploymentName
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--delete-policy")]
    public string? DeletePolicy { get; set; }
}