using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicebus", "queue", "create")]
public record AzServicebusQueueCreateOptions(
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

    [BooleanCommandSwitch("--enable-dead-lettering-on-message-expiration")]
    public bool? EnableDeadLetteringOnMessageExpiration { get; set; }

    [BooleanCommandSwitch("--enable-express")]
    public bool? EnableExpress { get; set; }

    [BooleanCommandSwitch("--enable-partitioning")]
    public bool? EnablePartitioning { get; set; }

    [BooleanCommandSwitch("--enable-session")]
    public bool? EnableSession { get; set; }

    [CommandSwitch("--forward-dead-lettered-messages-to")]
    public string? ForwardDeadLetteredMessagesTo { get; set; }

    [CommandSwitch("--forward-to")]
    public string? ForwardTo { get; set; }

    [CommandSwitch("--lock-duration")]
    public string? LockDuration { get; set; }

    [CommandSwitch("--max-delivery-count")]
    public int? MaxDeliveryCount { get; set; }

    [CommandSwitch("--max-message-size")]
    public string? MaxMessageSize { get; set; }

    [CommandSwitch("--max-size")]
    public string? MaxSize { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }
}