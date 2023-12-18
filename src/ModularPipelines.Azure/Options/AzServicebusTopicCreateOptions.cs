using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicebus", "topic", "create")]
public record AzServicebusTopicCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
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

    [CommandSwitch("--max-message-size")]
    public string? MaxMessageSize { get; set; }

    [CommandSwitch("--max-size")]
    public string? MaxSize { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }
}