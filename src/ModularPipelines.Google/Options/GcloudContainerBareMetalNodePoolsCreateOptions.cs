using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "bare-metal", "node-pools", "create")]
public record GcloudContainerBareMetalNodePoolsCreateOptions(
[property: PositionalArgument] string NodePool,
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--node-configs")] string[] NodeConfigs,
[property: CommandSwitch("--node-labels")] IEnumerable<KeyValue> NodeLabels,
[property: CommandSwitch("--node-taints")] IEnumerable<KeyValue> NodeTaints,
[property: BooleanCommandSwitch("--disable-serialize-image-pulls")] bool DisableSerializeImagePulls,
[property: CommandSwitch("--registry-burst")] string RegistryBurst,
[property: CommandSwitch("--registry-pull-qps")] string RegistryPullQps
) : GcloudOptions
{
    [CommandSwitch("--annotations")]
    public IEnumerable<KeyValue>? Annotations { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }
}