using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deployment-manager", "deployments", "cancel-preview")]
public record GcloudDeploymentManagerDeploymentsCancelPreviewOptions(
[property: CliArgument] string DeploymentName
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--fingerprint")]
    public string? Fingerprint { get; set; }
}