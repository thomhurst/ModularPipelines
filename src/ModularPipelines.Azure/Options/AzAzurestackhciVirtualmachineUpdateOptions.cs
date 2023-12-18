using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("urestackhci", "virtualmachine", "update")]
public record AzAzurestackhciVirtualmachineUpdateOptions : AzOptions
{
    [CommandSwitch("--cpu-count")]
    public int? CpuCount { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--memory-mb")]
    public string? MemoryMb { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--vhd-names")]
    public string? VhdNames { get; set; }

    [CommandSwitch("--vnic-names")]
    public string? VnicNames { get; set; }
}