using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicebus", "topic", "update")]
public record AzServicebusTopicUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--auto-delete-on-idle")]
    public string? AutoDeleteOnIdle { get; set; }

    [CommandSwitch("--default-message-time-to-live")]
    public string? DefaultMessageTimeToLive { get; set; }

    [BooleanCommandSwitch("--duplicate-detection")]
    public bool? DuplicateDetection { get; set; }

    [CommandSwitch("--duplicate-detection-history-time-window")]
    public string? DuplicateDetectionHistoryTimeWindow { get; set; }

    [BooleanCommandSwitch("--enable-batched-operations")]
    public bool? EnableBatchedOperations { get; set; }

    [BooleanCommandSwitch("--enable-express")]
    public bool? EnableExpress { get; set; }

    [BooleanCommandSwitch("--enable-ordering")]
    public bool? EnableOrdering { get; set; }

    [BooleanCommandSwitch("--enable-partitioning")]
    public bool? EnablePartitioning { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--max-message-size")]
    public string? MaxMessageSize { get; set; }

    [CommandSwitch("--max-size")]
    public string? MaxSize { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--namespace-name")]
    public string? NamespaceName { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}