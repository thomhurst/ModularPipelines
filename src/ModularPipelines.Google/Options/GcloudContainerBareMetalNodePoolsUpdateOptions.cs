using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "bare-metal", "node-pools", "update")]
public record GcloudContainerBareMetalNodePoolsUpdateOptions(
[property: PositionalArgument] string NodePool,
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--allow-missing")]
    public bool? AllowMissing { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CommandSwitch("--node-configs")]
    public string[]? NodeConfigs { get; set; }

    [CommandSwitch("--node-labels")]
    public IEnumerable<KeyValue>? NodeLabels { get; set; }

    [CommandSwitch("--node-taints")]
    public IEnumerable<KeyValue>? NodeTaints { get; set; }

    [CommandSwitch("--registry-burst")]
    public string? RegistryBurst { get; set; }

    [CommandSwitch("--registry-pull-qps")]
    public string? RegistryPullQps { get; set; }

    [BooleanCommandSwitch("--disable-serialize-image-pulls")]
    public bool? DisableSerializeImagePulls { get; set; }

    [BooleanCommandSwitch("--enable-serialize-image-pulls")]
    public bool? EnableSerializeImagePulls { get; set; }
}