using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "repair", "run")]
public record AzVmRepairRunOptions : AzOptions
{
    [CommandSwitch("--custom-script-file")]
    public string? CustomScriptFile { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--parameters")]
    public string? Parameters { get; set; }

    [CommandSwitch("--preview")]
    public string? Preview { get; set; }

    [CommandSwitch("--repair-vm-id")]
    public string? RepairVmId { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--run-id")]
    public string? RunId { get; set; }

    [BooleanCommandSwitch("--run-on-repair")]
    public bool? RunOnRepair { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

