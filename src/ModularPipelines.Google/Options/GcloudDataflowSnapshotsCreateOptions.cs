using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataflow", "snapshots", "create")]
public record GcloudDataflowSnapshotsCreateOptions(
[property: CommandSwitch("--job-id")] string JobId,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions
{
    [CommandSwitch("--snapshot-sources")]
    public string? SnapshotSources { get; set; }

    [CommandSwitch("--snapshot-ttl")]
    public string? SnapshotTtl { get; set; }
}