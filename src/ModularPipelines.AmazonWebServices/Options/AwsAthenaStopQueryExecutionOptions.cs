using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("athena", "stop-query-execution")]
public record AwsAthenaStopQueryExecutionOptions : AwsOptions
{
    [CliOption("--query-execution-id")]
    public string? QueryExecutionId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}