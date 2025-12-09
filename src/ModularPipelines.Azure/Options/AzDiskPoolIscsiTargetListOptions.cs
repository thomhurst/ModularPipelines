using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("disk-pool", "iscsi-target", "list")]
public record AzDiskPoolIscsiTargetListOptions(
[property: CliOption("--disk-pool-name")] string DiskPoolName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;