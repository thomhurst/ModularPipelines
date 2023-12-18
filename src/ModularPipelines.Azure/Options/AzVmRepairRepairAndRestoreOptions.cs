using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "repair", "repair-and-restore")]
public record AzVmRepairRepairAndRestoreOptions : AzOptions
{
    [CommandSwitch("--copy-disk-name")]
    public string? CopyDiskName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--repair-group-name")]
    public string? RepairGroupName { get; set; }

    [CommandSwitch("--repair-password")]
    public string? RepairPassword { get; set; }

    [CommandSwitch("--repair-username")]
    public string? RepairUsername { get; set; }

    [CommandSwitch("--repair-vm-name")]
    public string? RepairVmName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

