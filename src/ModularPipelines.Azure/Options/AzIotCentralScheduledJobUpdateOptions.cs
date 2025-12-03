using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "central", "scheduled-job", "update")]
public record AzIotCentralScheduledJobUpdateOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--id")] string Id
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

    [CliOption("--content")]
    public string? Content { get; set; }

    [CliOption("--desc")]
    public string? Desc { get; set; }

    [CliOption("--group-id")]
    public string? GroupId { get; set; }

    [CliOption("--job-name")]
    public string? JobName { get; set; }

    [CliOption("--schedule")]
    public string? Schedule { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}