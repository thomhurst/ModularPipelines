using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "bare-metal", "node-pools", "update")]
public record GcloudContainerBareMetalNodePoolsUpdateOptions(
[property: CliArgument] string NodePool,
[property: CliArgument] string Cluster,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--allow-missing")]
    public bool? AllowMissing { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CliOption("--node-configs")]
    public string[]? NodeConfigs { get; set; }

    [CliOption("--node-labels")]
    public IEnumerable<KeyValue>? NodeLabels { get; set; }

    [CliOption("--node-taints")]
    public IEnumerable<KeyValue>? NodeTaints { get; set; }

    [CliOption("--registry-burst")]
    public string? RegistryBurst { get; set; }

    [CliOption("--registry-pull-qps")]
    public string? RegistryPullQps { get; set; }

    [CliFlag("--disable-serialize-image-pulls")]
    public bool? DisableSerializeImagePulls { get; set; }

    [CliFlag("--enable-serialize-image-pulls")]
    public bool? EnableSerializeImagePulls { get; set; }
}