using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("disk-pool", "iscsi-target", "list")]
public record AzDiskPoolIscsiTargetListOptions(
[property: CommandSwitch("--disk-pool-name")] string DiskPoolName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;