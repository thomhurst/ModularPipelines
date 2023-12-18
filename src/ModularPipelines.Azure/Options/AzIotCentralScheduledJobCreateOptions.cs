using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "scheduled-job", "create")]
public record AzIotCentralScheduledJobCreateOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--content")] string Content,
[property: CommandSwitch("--group-id")] string GroupId,
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--schedule")] string Schedule
) : AzOptions
{
    [CommandSwitch("--api-version")]
    public string? ApiVersion { get; set; }

    [CommandSwitch("--batch")]
    public string? Batch { get; set; }

    [CommandSwitch("--batch-type")]
    public string? BatchType { get; set; }

    [CommandSwitch("--cancellation-threshold")]
    public string? CancellationThreshold { get; set; }

    [CommandSwitch("--cancellation-threshold-batch")]
    public string? CancellationThresholdBatch { get; set; }

    [CommandSwitch("--cancellation-threshold-type")]
    public string? CancellationThresholdType { get; set; }

    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CommandSwitch("--desc")]
    public string? Desc { get; set; }

    [CommandSwitch("--job-name")]
    public string? JobName { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}