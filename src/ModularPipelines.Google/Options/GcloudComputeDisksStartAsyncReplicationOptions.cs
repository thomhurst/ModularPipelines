using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "disks", "start-async-replication")]
public record GcloudComputeDisksStartAsyncReplicationOptions(
[property: PositionalArgument] string DiskName,
[property: CommandSwitch("--secondary-disk")] string SecondaryDisk,
[property: CommandSwitch("--region")] string Region,
[property: CommandSwitch("--zone")] string Zone,
[property: CommandSwitch("--secondary-disk-region")] string SecondaryDiskRegion,
[property: CommandSwitch("--secondary-disk-zone")] string SecondaryDiskZone
) : GcloudOptions;