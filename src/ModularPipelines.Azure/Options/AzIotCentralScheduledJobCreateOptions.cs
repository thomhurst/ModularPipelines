using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "central", "scheduled-job", "create")]
public record AzIotCentralScheduledJobCreateOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--content")] string Content,
[property: CliOption("--group-id")] string GroupId,
[property: CliOption("--id")] string Id,
[property: CliOption("--schedule")] string Schedule
) : AzOptions
{
    [CliOption("--batch")]
    public string? Batch { get; set; }

    [CliOption("--batch-type")]
    public string? BatchType { get; set; }

    [CliOption("--cancellation-threshold")]
    public string? CancellationThreshold { get; set; }

    [CliOption("--cancellation-threshold-batch")]
    public string? CancellationThresholdBatch { get; set; }

    [CliOption("--cancellation-threshold-type")]
    public string? CancellationThresholdType { get; set; }

    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--desc")]
    public string? Desc { get; set; }

    [CliOption("--job-name")]
    public string? JobName { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}