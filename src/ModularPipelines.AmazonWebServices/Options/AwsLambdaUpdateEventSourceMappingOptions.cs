using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lambda", "update-event-source-mapping")]
public record AwsLambdaUpdateEventSourceMappingOptions(
[property: CommandSwitch("--uuid")] string Uuid
) : AwsOptions
{
    [CommandSwitch("--function-name")]
    public string? FunctionName { get; set; }

    [CommandSwitch("--batch-size")]
    public int? BatchSize { get; set; }

    [CommandSwitch("--filter-criteria")]
    public string? FilterCriteria { get; set; }

    [CommandSwitch("--maximum-batching-window-in-seconds")]
    public int? MaximumBatchingWindowInSeconds { get; set; }

    [CommandSwitch("--destination-config")]
    public string? DestinationConfig { get; set; }

    [CommandSwitch("--maximum-record-age-in-seconds")]
    public int? MaximumRecordAgeInSeconds { get; set; }

    [CommandSwitch("--maximum-retry-attempts")]
    public int? MaximumRetryAttempts { get; set; }

    [CommandSwitch("--parallelization-factor")]
    public int? ParallelizationFactor { get; set; }

    [CommandSwitch("--source-access-configurations")]
    public string[]? SourceAccessConfigurations { get; set; }

    [CommandSwitch("--tumbling-window-in-seconds")]
    public int? TumblingWindowInSeconds { get; set; }

    [CommandSwitch("--function-response-types")]
    public string[]? FunctionResponseTypes { get; set; }

    [CommandSwitch("--scaling-config")]
    public string? ScalingConfig { get; set; }

    [CommandSwitch("--document-db-event-source-config")]
    public string? DocumentDbEventSourceConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}