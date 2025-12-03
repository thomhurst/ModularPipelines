using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "clusters", "upgrade")]
public record GcloudContainerClustersUpgradeOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--cluster-version")]
    public string? ClusterVersion { get; set; }

    [CliOption("--image-type")]
    public string? ImageType { get; set; }

    [CliFlag("--master")]
    public bool? Master { get; set; }

    [CliOption("--node-pool")]
    public string? NodePool { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}