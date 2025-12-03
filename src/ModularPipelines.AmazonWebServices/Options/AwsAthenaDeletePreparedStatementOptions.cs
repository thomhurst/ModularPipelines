using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("athena", "delete-prepared-statement")]
public record AwsAthenaDeletePreparedStatementOptions(
[property: CliOption("--statement-name")] string StatementName,
[property: CliOption("--work-group")] string WorkGroup
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}