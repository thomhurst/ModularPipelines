using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lambda", "create-event-source-mapping")]
public record AwsLambdaCreateEventSourceMappingOptions(
[property: CommandSwitch("--function-name")] string FunctionName
) : AwsOptions
{
    [CommandSwitch("--event-source-arn")]
    public string? EventSourceArn { get; set; }

    [CommandSwitch("--batch-size")]
    public int? BatchSize { get; set; }

    [CommandSwitch("--filter-criteria")]
    public string? FilterCriteria { get; set; }

    [CommandSwitch("--maximum-batching-window-in-seconds")]
    public int? MaximumBatchingWindowInSeconds { get; set; }

    [CommandSwitch("--parallelization-factor")]
    public int? ParallelizationFactor { get; set; }

    [CommandSwitch("--starting-position")]
    public string? StartingPosition { get; set; }

    [CommandSwitch("--starting-position-timestamp")]
    public long? StartingPositionTimestamp { get; set; }

    [CommandSwitch("--destination-config")]
    public string? DestinationConfig { get; set; }

    [CommandSwitch("--maximum-record-age-in-seconds")]
    public int? MaximumRecordAgeInSeconds { get; set; }

    [CommandSwitch("--maximum-retry-attempts")]
    public int? MaximumRetryAttempts { get; set; }

    [CommandSwitch("--tumbling-window-in-seconds")]
    public int? TumblingWindowInSeconds { get; set; }

    [CommandSwitch("--topics")]
    public string[]? Topics { get; set; }

    [CommandSwitch("--queues")]
    public string[]? Queues { get; set; }

    [CommandSwitch("--source-access-configurations")]
    public string[]? SourceAccessConfigurations { get; set; }

    [CommandSwitch("--self-managed-event-source")]
    public string? SelfManagedEventSource { get; set; }

    [CommandSwitch("--function-response-types")]
    public string[]? FunctionResponseTypes { get; set; }

    [CommandSwitch("--amazon-managed-kafka-event-source-config")]
    public string? AmazonManagedKafkaEventSourceConfig { get; set; }

    [CommandSwitch("--self-managed-kafka-event-source-config")]
    public string? SelfManagedKafkaEventSourceConfig { get; set; }

    [CommandSwitch("--scaling-config")]
    public string? ScalingConfig { get; set; }

    [CommandSwitch("--document-db-event-source-config")]
    public string? DocumentDbEventSourceConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}