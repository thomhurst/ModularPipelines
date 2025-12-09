using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("athena", "batch-get-prepared-statement")]
public record AwsAthenaBatchGetPreparedStatementOptions(
[property: CliOption("--prepared-statement-names")] string[] PreparedStatementNames,
[property: CliOption("--work-group")] string WorkGroup
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}