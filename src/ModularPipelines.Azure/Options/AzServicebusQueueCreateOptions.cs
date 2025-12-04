using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("servicebus", "queue", "create")]
public record AzServicebusQueueCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--auto-delete-on-idle")]
    public string? AutoDeleteOnIdle { get; set; }

    [CliOption("--default-message-time-to-live")]
    public string? DefaultMessageTimeToLive { get; set; }

    [CliFlag("--duplicate-detection")]
    public bool? DuplicateDetection { get; set; }

    [CliOption("--duplicate-detection-history-time-window")]
    public string? DuplicateDetectionHistoryTimeWindow { get; set; }

    [CliFlag("--enable-batched-operations")]
    public bool? EnableBatchedOperations { get; set; }

    [CliFlag("--enable-dead-lettering-on-message-expiration")]
    public bool? EnableDeadLetteringOnMessageExpiration { get; set; }

    [CliFlag("--enable-express")]
    public bool? EnableExpress { get; set; }

    [CliFlag("--enable-partitioning")]
    public bool? EnablePartitioning { get; set; }

    [CliFlag("--enable-session")]
    public bool? EnableSession { get; set; }

    [CliOption("--forward-dead-lettered-messages-to")]
    public string? ForwardDeadLetteredMessagesTo { get; set; }

    [CliOption("--forward-to")]
    public string? ForwardTo { get; set; }

    [CliOption("--lock-duration")]
    public string? LockDuration { get; set; }

    [CliOption("--max-delivery-count")]
    public int? MaxDeliveryCount { get; set; }

    [CliOption("--max-message-size")]
    public string? MaxMessageSize { get; set; }

    [CliOption("--max-size")]
    public string? MaxSize { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }
}