using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vm", "repair", "repair-and-restore")]
public record AzVmRepairRepairAndRestoreOptions : AzOptions
{
    [CliOption("--copy-disk-name")]
    public string? CopyDiskName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--repair-group-name")]
    public string? RepairGroupName { get; set; }

    [CliOption("--repair-password")]
    public string? RepairPassword { get; set; }

    [CliOption("--repair-username")]
    public string? RepairUsername { get; set; }

    [CliOption("--repair-vm-name")]
    public string? RepairVmName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}