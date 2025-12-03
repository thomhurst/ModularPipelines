using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bms", "volumes", "snapshots", "list")]
public record GcloudBmsVolumesSnapshotsListOptions(
[property: CliOption("--volume")] string Volume,
[property: CliOption("--region")] string Region
) : GcloudOptions;