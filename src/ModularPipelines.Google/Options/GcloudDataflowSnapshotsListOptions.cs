using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataflow", "snapshots", "list")]
public record GcloudDataflowSnapshotsListOptions(
[property: CliOption("--region")] string Region
) : GcloudOptions
{
    [CliOption("--job-id")]
    public string? JobId { get; set; }
}