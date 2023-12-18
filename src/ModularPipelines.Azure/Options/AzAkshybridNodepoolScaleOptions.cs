using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("akshybrid", "nodepool", "scale")]
public record AzAkshybridNodepoolScaleOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--node-count")]
    public int? NodeCount { get; set; }

    [CommandSwitch("--node-vm-size")]
    public string? NodeVmSize { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}