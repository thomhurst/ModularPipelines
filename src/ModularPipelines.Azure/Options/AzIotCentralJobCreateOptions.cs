using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "job", "create")]
public record AzIotCentralJobCreateOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--content")] string Content,
[property: CommandSwitch("--group-id")] string GroupId,
[property: CommandSwitch("--job-id")] string JobId
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

    [CommandSwitch("--desc")]
    public string? Desc { get; set; }

    [CommandSwitch("--job-name")]
    public string? JobName { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}