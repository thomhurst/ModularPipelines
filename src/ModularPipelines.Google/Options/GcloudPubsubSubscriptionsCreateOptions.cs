using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "subscriptions", "create")]
public record GcloudPubsubSubscriptionsCreateOptions(
[property: CliArgument] string Subscription,
[property: CliOption("--topic")] string Topic,
[property: CliOption("--topic-project")] string TopicProject
) : GcloudOptions
{
    [CliOption("--ack-deadline")]
    public string? AckDeadline { get; set; }

    [CliFlag("--enable-exactly-once-delivery")]
    public bool? EnableExactlyOnceDelivery { get; set; }

    [CliFlag("--enable-message-ordering")]
    public bool? EnableMessageOrdering { get; set; }

    [CliOption("--expiration-period")]
    public string? ExpirationPeriod { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--message-filter")]
    public string? MessageFilter { get; set; }

    [CliOption("--message-retention-duration")]
    public string? MessageRetentionDuration { get; set; }

    [CliOption("--push-auth-service-account")]
    public string? PushAuthServiceAccount { get; set; }

    [CliOption("--push-auth-token-audience")]
    public string? PushAuthTokenAudience { get; set; }

    [CliOption("--push-endpoint")]
    public string? PushEndpoint { get; set; }

    [CliFlag("--retain-acked-messages")]
    public bool? RetainAckedMessages { get; set; }

    [CliOption("--bigquery-table")]
    public string? BigqueryTable { get; set; }

    [CliFlag("--drop-unknown-fields")]
    public bool? DropUnknownFields { get; set; }

    [CliFlag("--write-metadata")]
    public bool? WriteMetadata { get; set; }

    [CliFlag("--use-table-schema")]
    public bool? UseTableSchema { get; set; }

    [CliFlag("--use-topic-schema")]
    public bool? UseTopicSchema { get; set; }

    [CliOption("--cloud-storage-bucket")]
    public string? CloudStorageBucket { get; set; }

    [CliOption("--cloud-storage-file-prefix")]
    public string? CloudStorageFilePrefix { get; set; }

    [CliOption("--cloud-storage-file-suffix")]
    public string? CloudStorageFileSuffix { get; set; }

    [CliOption("--cloud-storage-max-bytes")]
    public string? CloudStorageMaxBytes { get; set; }

    [CliOption("--cloud-storage-max-duration")]
    public string? CloudStorageMaxDuration { get; set; }

    [CliOption("--cloud-storage-output-format")]
    public string? CloudStorageOutputFormat { get; set; }

    [CliFlag("--cloud-storage-write-metadata")]
    public bool? CloudStorageWriteMetadata { get; set; }

    [CliOption("--max-delivery-attempts")]
    public string? MaxDeliveryAttempts { get; set; }

    [CliOption("--dead-letter-topic")]
    public string? DeadLetterTopic { get; set; }

    [CliOption("--dead-letter-topic-project")]
    public string? DeadLetterTopicProject { get; set; }

    [CliOption("--max-retry-delay")]
    public string? MaxRetryDelay { get; set; }

    [CliOption("--min-retry-delay")]
    public string? MinRetryDelay { get; set; }

    [CliFlag("--push-no-wrapper")]
    public bool? PushNoWrapper { get; set; }

    [CliFlag("--push-no-wrapper-write-metadata")]
    public bool? PushNoWrapperWriteMetadata { get; set; }
}