using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "disks", "start-async-replication")]
public record GcloudComputeDisksStartAsyncReplicationOptions(
[property: CliArgument] string DiskName,
[property: CliOption("--secondary-disk")] string SecondaryDisk,
[property: CliOption("--region")] string Region,
[property: CliOption("--zone")] string Zone,
[property: CliOption("--secondary-disk-region")] string SecondaryDiskRegion,
[property: CliOption("--secondary-disk-zone")] string SecondaryDiskZone
) : GcloudOptions;