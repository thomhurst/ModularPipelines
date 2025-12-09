using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vm", "repair", "run")]
public record AzVmRepairRunOptions : AzOptions
{
    [CliOption("--custom-script-file")]
    public string? CustomScriptFile { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--preview")]
    public string? Preview { get; set; }

    [CliOption("--repair-vm-id")]
    public string? RepairVmId { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--run-id")]
    public string? RunId { get; set; }

    [CliFlag("--run-on-repair")]
    public bool? RunOnRepair { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}