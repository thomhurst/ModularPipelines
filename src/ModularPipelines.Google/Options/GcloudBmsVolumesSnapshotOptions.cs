using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bms", "volumes", "snapshot")]
public record GcloudBmsVolumesSnapshotOptions(
[property: CliArgument] string Volume,
[property: CliArgument] string Region,
[property: CliOption("--description")] string Description,
[property: CliOption("--snapshot-name")] string SnapshotName
) : GcloudOptions;