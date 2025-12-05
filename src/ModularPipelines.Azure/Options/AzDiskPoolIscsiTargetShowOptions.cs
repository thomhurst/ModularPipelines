using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("disk-pool", "iscsi-target", "show")]
public record AzDiskPoolIscsiTargetShowOptions : AzOptions
{
    [CliOption("--disk-pool-name")]
    public string? DiskPoolName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--iscsi-target-name")]
    public string? IscsiTargetName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}