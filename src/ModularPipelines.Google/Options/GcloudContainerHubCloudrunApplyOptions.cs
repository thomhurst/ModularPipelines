using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "cloudrun", "apply")]
public record GcloudContainerHubCloudrunApplyOptions(
[property: CommandSwitch("--gke-cluster")] string GkeCluster,
[property: CommandSwitch("--gke-uri")] string GkeUri,
[property: CommandSwitch("--context")] string Context,
[property: CommandSwitch("--kubeconfig")] string Kubeconfig
) : GcloudOptions
{
    [CommandSwitch("--config")]
    public string? Config { get; set; }
}