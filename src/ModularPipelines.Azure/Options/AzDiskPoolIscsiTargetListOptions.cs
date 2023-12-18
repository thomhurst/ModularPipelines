using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("disk-pool", "iscsi-target", "list")]
public record AzDiskPoolIscsiTargetListOptions(
[property: CommandSwitch("--disk-pool-name")] string DiskPoolName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--iscsi-target-name")]
    public string? IscsiTargetName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}