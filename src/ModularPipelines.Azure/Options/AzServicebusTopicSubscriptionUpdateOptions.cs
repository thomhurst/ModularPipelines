using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicebus", "topic", "subscription", "update")]
public record AzServicebusTopicSubscriptionUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--auto-delete-on-idle")]
    public string? AutoDeleteOnIdle { get; set; }

    [CommandSwitch("--client-id")]
    public string? ClientId { get; set; }

    [BooleanCommandSwitch("--dead-letter-on-filter-exceptions")]
    public bool? DeadLetterOnFilterExceptions { get; set; }

    [CommandSwitch("--default-message-time-to-live")]
    public string? DefaultMessageTimeToLive { get; set; }

    [CommandSwitch("--duplicate-detection-history-time-window")]
    public string? DuplicateDetectionHistoryTimeWindow { get; set; }

    [BooleanCommandSwitch("--enable-batched-operations")]
    public bool? EnableBatchedOperations { get; set; }

    [BooleanCommandSwitch("--enable-dead-lettering-on-message-expiration")]
    public bool? EnableDeadLetteringOnMessageExpiration { get; set; }

    [BooleanCommandSwitch("--enable-session")]
    public bool? EnableSession { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--forward-dead-lettered-messages-to")]
    public string? ForwardDeadLetteredMessagesTo { get; set; }

    [CommandSwitch("--forward-to")]
    public string? ForwardTo { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--is-client-affine")]
    public bool? IsClientAffine { get; set; }

    [BooleanCommandSwitch("--is-durable")]
    public bool? IsDurable { get; set; }

    [BooleanCommandSwitch("--is-shared")]
    public bool? IsShared { get; set; }

    [CommandSwitch("--lock-duration")]
    public string? LockDuration { get; set; }

    [CommandSwitch("--max-delivery-count")]
    public int? MaxDeliveryCount { get; set; }

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

    [CommandSwitch("--topic-name")]
    public string? TopicName { get; set; }
}