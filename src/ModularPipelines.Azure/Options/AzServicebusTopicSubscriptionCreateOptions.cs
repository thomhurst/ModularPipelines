using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicebus", "topic", "subscription", "create")]
public record AzServicebusTopicSubscriptionCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--topic-name")] string TopicName
) : AzOptions
{
    [CliOption("--auto-delete-on-idle")]
    public string? AutoDeleteOnIdle { get; set; }

    [CliOption("--client-id")]
    public string? ClientId { get; set; }

    [CliFlag("--dead-letter-on-filter-exceptions")]
    public bool? DeadLetterOnFilterExceptions { get; set; }

    [CliOption("--default-message-time-to-live")]
    public string? DefaultMessageTimeToLive { get; set; }

    [CliOption("--duplicate-detection-history-time-window")]
    public string? DuplicateDetectionHistoryTimeWindow { get; set; }

    [CliFlag("--enable-batched-operations")]
    public bool? EnableBatchedOperations { get; set; }

    [CliFlag("--enable-dead-lettering-on-message-expiration")]
    public bool? EnableDeadLetteringOnMessageExpiration { get; set; }

    [CliFlag("--enable-session")]
    public bool? EnableSession { get; set; }

    [CliOption("--forward-dead-lettered-messages-to")]
    public string? ForwardDeadLetteredMessagesTo { get; set; }

    [CliOption("--forward-to")]
    public string? ForwardTo { get; set; }

    [CliFlag("--is-client-affine")]
    public bool? IsClientAffine { get; set; }

    [CliFlag("--is-durable")]
    public bool? IsDurable { get; set; }

    [CliFlag("--is-shared")]
    public bool? IsShared { get; set; }

    [CliOption("--lock-duration")]
    public string? LockDuration { get; set; }

    [CliOption("--max-delivery-count")]
    public int? MaxDeliveryCount { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }
}