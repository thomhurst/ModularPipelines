using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment-manager", "deployments", "cancel-preview")]
public record GcloudDeploymentManagerDeploymentsCancelPreviewOptions(
[property: PositionalArgument] string DeploymentName
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--fingerprint")]
    public string? Fingerprint { get; set; }
}