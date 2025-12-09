using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("athena", "get-query-runtime-statistics")]
public record AwsAthenaGetQueryRuntimeStatisticsOptions(
[property: CliOption("--query-execution-id")] string QueryExecutionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}