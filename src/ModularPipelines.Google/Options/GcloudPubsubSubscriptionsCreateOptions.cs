using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "subscriptions", "create")]
public record GcloudPubsubSubscriptionsCreateOptions(
[property: PositionalArgument] string Subscription,
[property: CommandSwitch("--topic")] string Topic,
[property: CommandSwitch("--topic-project")] string TopicProject
) : GcloudOptions
{
    [CommandSwitch("--ack-deadline")]
    public string? AckDeadline { get; set; }

    [BooleanCommandSwitch("--enable-exactly-once-delivery")]
    public bool? EnableExactlyOnceDelivery { get; set; }

    [BooleanCommandSwitch("--enable-message-ordering")]
    public bool? EnableMessageOrdering { get; set; }

    [CommandSwitch("--expiration-period")]
    public string? ExpirationPeriod { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--message-filter")]
    public string? MessageFilter { get; set; }

    [CommandSwitch("--message-retention-duration")]
    public string? MessageRetentionDuration { get; set; }

    [CommandSwitch("--push-auth-service-account")]
    public string? PushAuthServiceAccount { get; set; }

    [CommandSwitch("--push-auth-token-audience")]
    public string? PushAuthTokenAudience { get; set; }

    [CommandSwitch("--push-endpoint")]
    public string? PushEndpoint { get; set; }

    [BooleanCommandSwitch("--retain-acked-messages")]
    public bool? RetainAckedMessages { get; set; }

    [CommandSwitch("--bigquery-table")]
    public string? BigqueryTable { get; set; }

    [BooleanCommandSwitch("--drop-unknown-fields")]
    public bool? DropUnknownFields { get; set; }

    [BooleanCommandSwitch("--write-metadata")]
    public bool? WriteMetadata { get; set; }

    [BooleanCommandSwitch("--use-table-schema")]
    public bool? UseTableSchema { get; set; }

    [BooleanCommandSwitch("--use-topic-schema")]
    public bool? UseTopicSchema { get; set; }

    [CommandSwitch("--cloud-storage-bucket")]
    public string? CloudStorageBucket { get; set; }

    [CommandSwitch("--cloud-storage-file-prefix")]
    public string? CloudStorageFilePrefix { get; set; }

    [CommandSwitch("--cloud-storage-file-suffix")]
    public string? CloudStorageFileSuffix { get; set; }

    [CommandSwitch("--cloud-storage-max-bytes")]
    public string? CloudStorageMaxBytes { get; set; }

    [CommandSwitch("--cloud-storage-max-duration")]
    public string? CloudStorageMaxDuration { get; set; }

    [CommandSwitch("--cloud-storage-output-format")]
    public string? CloudStorageOutputFormat { get; set; }

    [BooleanCommandSwitch("--cloud-storage-write-metadata")]
    public bool? CloudStorageWriteMetadata { get; set; }

    [CommandSwitch("--max-delivery-attempts")]
    public string? MaxDeliveryAttempts { get; set; }

    [CommandSwitch("--dead-letter-topic")]
    public string? DeadLetterTopic { get; set; }

    [CommandSwitch("--dead-letter-topic-project")]
    public string? DeadLetterTopicProject { get; set; }

    [CommandSwitch("--max-retry-delay")]
    public string? MaxRetryDelay { get; set; }

    [CommandSwitch("--min-retry-delay")]
    public string? MinRetryDelay { get; set; }

    [BooleanCommandSwitch("--push-no-wrapper")]
    public bool? PushNoWrapper { get; set; }

    [BooleanCommandSwitch("--push-no-wrapper-write-metadata")]
    public bool? PushNoWrapperWriteMetadata { get; set; }
}