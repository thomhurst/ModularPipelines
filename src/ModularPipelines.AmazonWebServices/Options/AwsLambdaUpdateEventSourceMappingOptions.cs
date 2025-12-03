using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lambda", "update-event-source-mapping")]
public record AwsLambdaUpdateEventSourceMappingOptions(
[property: CliOption("--uuid")] string Uuid
) : AwsOptions
{
    [CliOption("--function-name")]
    public string? FunctionName { get; set; }

    [CliOption("--batch-size")]
    public int? BatchSize { get; set; }

    [CliOption("--filter-criteria")]
    public string? FilterCriteria { get; set; }

    [CliOption("--maximum-batching-window-in-seconds")]
    public int? MaximumBatchingWindowInSeconds { get; set; }

    [CliOption("--destination-config")]
    public string? DestinationConfig { get; set; }

    [CliOption("--maximum-record-age-in-seconds")]
    public int? MaximumRecordAgeInSeconds { get; set; }

    [CliOption("--maximum-retry-attempts")]
    public int? MaximumRetryAttempts { get; set; }

    [CliOption("--parallelization-factor")]
    public int? ParallelizationFactor { get; set; }

    [CliOption("--source-access-configurations")]
    public string[]? SourceAccessConfigurations { get; set; }

    [CliOption("--tumbling-window-in-seconds")]
    public int? TumblingWindowInSeconds { get; set; }

    [CliOption("--function-response-types")]
    public string[]? FunctionResponseTypes { get; set; }

    [CliOption("--scaling-config")]
    public string? ScalingConfig { get; set; }

    [CliOption("--document-db-event-source-config")]
    public string? DocumentDbEventSourceConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}