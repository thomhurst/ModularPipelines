using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("disk-pool", "iscsi-target", "delete")]
public record AzDiskPoolIscsiTargetDeleteOptions : AzOptions
{
    [CliOption("--disk-pool-name")]
    public string? DiskPoolName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--iscsi-target-name")]
    public string? IscsiTargetName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}