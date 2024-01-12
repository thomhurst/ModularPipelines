using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bigtable", "instances", "create")]
public record GcloudBigtableInstancesCreateOptions(
[property: PositionalArgument] string Instance,
[property: CommandSwitch("--display-name")] string DisplayName
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--cluster")]
    public string? Cluster { get; set; }

    [CommandSwitch("--cluster-config")]
    public string[]? ClusterConfig { get; set; }

    [CommandSwitch("--cluster-num-nodes")]
    public string? ClusterNumNodes { get; set; }

    [CommandSwitch("--cluster-storage-type")]
    public string? ClusterStorageType { get; set; }

    [CommandSwitch("--cluster-zone")]
    public string? ClusterZone { get; set; }

    [CommandSwitch("--instance-type")]
    public string? InstanceType { get; set; }
}