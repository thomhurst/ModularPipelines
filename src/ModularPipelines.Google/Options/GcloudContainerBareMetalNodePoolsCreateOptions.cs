using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "bare-metal", "node-pools", "create")]
public record GcloudContainerBareMetalNodePoolsCreateOptions(
[property: CliArgument] string NodePool,
[property: CliArgument] string Cluster,
[property: CliArgument] string Location,
[property: CliOption("--node-configs")] string[] NodeConfigs,
[property: CliOption("--node-labels")] IEnumerable<KeyValue> NodeLabels,
[property: CliOption("--node-taints")] IEnumerable<KeyValue> NodeTaints,
[property: CliFlag("--disable-serialize-image-pulls")] bool DisableSerializeImagePulls,
[property: CliOption("--registry-burst")] string RegistryBurst,
[property: CliOption("--registry-pull-qps")] string RegistryPullQps
) : GcloudOptions
{
    [CliOption("--annotations")]
    public IEnumerable<KeyValue>? Annotations { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }
}