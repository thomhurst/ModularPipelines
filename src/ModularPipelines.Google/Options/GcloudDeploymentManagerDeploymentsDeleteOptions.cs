using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment-manager", "deployments", "delete")]
public record GcloudDeploymentManagerDeploymentsDeleteOptions(
[property: PositionalArgument] string DeploymentName
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--delete-policy")]
    public string? DeletePolicy { get; set; }
}