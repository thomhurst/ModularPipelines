using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "scheduled-job", "update")]
public record AzIotCentralScheduledJobUpdateOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--id")] string Id
) : AzOptions
{
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

    [CommandSwitch("--content")]
    public string? Content { get; set; }

    [CommandSwitch("--desc")]
    public string? Desc { get; set; }

    [CommandSwitch("--group-id")]
    public string? GroupId { get; set; }

    [CommandSwitch("--job-name")]
    public string? JobName { get; set; }

    [CommandSwitch("--schedule")]
    public string? Schedule { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}