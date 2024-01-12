using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataflow", "snapshots", "list")]
public record GcloudDataflowSnapshotsListOptions(
[property: CommandSwitch("--region")] string Region
) : GcloudOptions
{
    [CommandSwitch("--job-id")]
    public string? JobId { get; set; }
}