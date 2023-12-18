using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aksarc", "nodepool", "add")]
public record AzAksarcNodepoolAddOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--labels")]
    public string? Labels { get; set; }

    [CommandSwitch("--max-pods")]
    public string? MaxPods { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--node-count")]
    public int? NodeCount { get; set; }

    [CommandSwitch("--node-taints")]
    public string? NodeTaints { get; set; }

    [CommandSwitch("--node-vm-size")]
    public string? NodeVmSize { get; set; }

    [CommandSwitch("--os-sku")]
    public string? OsSku { get; set; }

    [CommandSwitch("--os-type")]
    public string? OsType { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

