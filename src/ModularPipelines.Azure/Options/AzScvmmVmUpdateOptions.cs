using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scvmm", "vm", "update")]
public record AzScvmmVmUpdateOptions : AzOptions
{
    [CommandSwitch("--availability-sets")]
    public string? AvailabilitySets { get; set; }

    [CommandSwitch("--cpu-count")]
    public int? CpuCount { get; set; }

    [BooleanCommandSwitch("--dynamic-memory-enabled")]
    public bool? DynamicMemoryEnabled { get; set; }

    [CommandSwitch("--dynamic-memory-max")]
    public string? DynamicMemoryMax { get; set; }

    [CommandSwitch("--dynamic-memory-min")]
    public string? DynamicMemoryMin { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--memory-size")]
    public string? MemorySize { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}