using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataflow", "snapshots", "create")]
public record GcloudDataflowSnapshotsCreateOptions(
[property: CliOption("--job-id")] string JobId,
[property: CliOption("--region")] string Region
) : GcloudOptions
{
    [CliOption("--snapshot-sources")]
    public string? SnapshotSources { get; set; }

    [CliOption("--snapshot-ttl")]
    public string? SnapshotTtl { get; set; }
}