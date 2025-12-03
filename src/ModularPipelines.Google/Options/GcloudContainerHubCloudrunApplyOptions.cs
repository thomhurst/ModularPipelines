using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "cloudrun", "apply")]
public record GcloudContainerHubCloudrunApplyOptions(
[property: CliOption("--gke-cluster")] string GkeCluster,
[property: CliOption("--gke-uri")] string GkeUri,
[property: CliOption("--context")] string Context,
[property: CliOption("--kubeconfig")] string Kubeconfig
) : GcloudOptions
{
    [CliOption("--config")]
    public string? Config { get; set; }
}