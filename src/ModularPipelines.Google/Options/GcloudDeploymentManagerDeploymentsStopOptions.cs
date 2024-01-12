using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment-manager", "deployments", "stop")]
public record GcloudDeploymentManagerDeploymentsStopOptions(
[property: PositionalArgument] string DeploymentName
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--fingerprint")]
    public string? Fingerprint { get; set; }
}