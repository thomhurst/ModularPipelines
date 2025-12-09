using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lambda", "create-event-source-mapping")]
public record AwsLambdaCreateEventSourceMappingOptions(
[property: CliOption("--function-name")] string FunctionName
) : AwsOptions
{
    [CliOption("--event-source-arn")]
    public string? EventSourceArn { get; set; }

    [CliOption("--batch-size")]
    public int? BatchSize { get; set; }

    [CliOption("--filter-criteria")]
    public string? FilterCriteria { get; set; }

    [CliOption("--maximum-batching-window-in-seconds")]
    public int? MaximumBatchingWindowInSeconds { get; set; }

    [CliOption("--parallelization-factor")]
    public int? ParallelizationFactor { get; set; }

    [CliOption("--starting-position")]
    public string? StartingPosition { get; set; }

    [CliOption("--starting-position-timestamp")]
    public long? StartingPositionTimestamp { get; set; }

    [CliOption("--destination-config")]
    public string? DestinationConfig { get; set; }

    [CliOption("--maximum-record-age-in-seconds")]
    public int? MaximumRecordAgeInSeconds { get; set; }

    [CliOption("--maximum-retry-attempts")]
    public int? MaximumRetryAttempts { get; set; }

    [CliOption("--tumbling-window-in-seconds")]
    public int? TumblingWindowInSeconds { get; set; }

    [CliOption("--topics")]
    public string[]? Topics { get; set; }

    [CliOption("--queues")]
    public string[]? Queues { get; set; }

    [CliOption("--source-access-configurations")]
    public string[]? SourceAccessConfigurations { get; set; }

    [CliOption("--self-managed-event-source")]
    public string? SelfManagedEventSource { get; set; }

    [CliOption("--function-response-types")]
    public string[]? FunctionResponseTypes { get; set; }

    [CliOption("--amazon-managed-kafka-event-source-config")]
    public string? AmazonManagedKafkaEventSourceConfig { get; set; }

    [CliOption("--self-managed-kafka-event-source-config")]
    public string? SelfManagedKafkaEventSourceConfig { get; set; }

    [CliOption("--scaling-config")]
    public string? ScalingConfig { get; set; }

    [CliOption("--document-db-event-source-config")]
    public string? DocumentDbEventSourceConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}