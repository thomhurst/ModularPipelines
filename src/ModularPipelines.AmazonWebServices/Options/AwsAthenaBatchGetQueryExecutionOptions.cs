using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("athena", "batch-get-query-execution")]
public record AwsAthenaBatchGetQueryExecutionOptions(
[property: CliOption("--query-execution-ids")] string[] QueryExecutionIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}