using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicebus", "queue", "update")]
public record AzServicebusQueueUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--auto-delete-on-idle")]
    public string? AutoDeleteOnIdle { get; set; }

    [CliOption("--default-message-time-to-live")]
    public string? DefaultMessageTimeToLive { get; set; }

    [CliOption("--duplicate-detection-history-time-window")]
    public string? DuplicateDetectionHistoryTimeWindow { get; set; }

    [CliFlag("--enable-batched-operations")]
    public bool? EnableBatchedOperations { get; set; }

    [CliFlag("--enable-dead-lettering-on-message-expiration")]
    public bool? EnableDeadLetteringOnMessageExpiration { get; set; }

    [CliFlag("--enable-express")]
    public bool? EnableExpress { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--forward-dead-lettered-messages-to")]
    public string? ForwardDeadLetteredMessagesTo { get; set; }

    [CliOption("--forward-to")]
    public string? ForwardTo { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--lock-duration")]
    public string? LockDuration { get; set; }

    [CliOption("--max-delivery-count")]
    public int? MaxDeliveryCount { get; set; }

    [CliOption("--max-message-size")]
    public string? MaxMessageSize { get; set; }

    [CliOption("--max-size")]
    public string? MaxSize { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--namespace-name")]
    public string? NamespaceName { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}