using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "subscriptions", "update")]
public record GcloudPubsubSubscriptionsUpdateOptions(
[property: PositionalArgument] string Subscription
) : GcloudOptions
{
    [CommandSwitch("--ack-deadline")]
    public string? AckDeadline { get; set; }

    [BooleanCommandSwitch("--enable-exactly-once-delivery")]
    public bool? EnableExactlyOnceDelivery { get; set; }

    [CommandSwitch("--expiration-period")]
    public string? ExpirationPeriod { get; set; }

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

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [BooleanCommandSwitch("--clear-bigquery-config")]
    public bool? ClearBigqueryConfig { get; set; }

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

    [BooleanCommandSwitch("--clear-cloud-storage-config")]
    public bool? ClearCloudStorageConfig { get; set; }

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

    [BooleanCommandSwitch("--clear-dead-letter-policy")]
    public bool? ClearDeadLetterPolicy { get; set; }

    [CommandSwitch("--max-delivery-attempts")]
    public string? MaxDeliveryAttempts { get; set; }

    [CommandSwitch("--dead-letter-topic")]
    public string? DeadLetterTopic { get; set; }

    [CommandSwitch("--dead-letter-topic-project")]
    public string? DeadLetterTopicProject { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [BooleanCommandSwitch("--clear-push-no-wrapper-config")]
    public bool? ClearPushNoWrapperConfig { get; set; }

    [BooleanCommandSwitch("--push-no-wrapper")]
    public bool? PushNoWrapper { get; set; }

    [BooleanCommandSwitch("--push-no-wrapper-write-metadata")]
    public bool? PushNoWrapperWriteMetadata { get; set; }

    [BooleanCommandSwitch("--clear-retry-policy")]
    public bool? ClearRetryPolicy { get; set; }

    [CommandSwitch("--max-retry-delay")]
    public string? MaxRetryDelay { get; set; }

    [CommandSwitch("--min-retry-delay")]
    public string? MinRetryDelay { get; set; }
}