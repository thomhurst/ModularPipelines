using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("athena", "create-prepared-statement")]
public record AwsAthenaCreatePreparedStatementOptions(
[property: CliOption("--statement-name")] string StatementName,
[property: CliOption("--work-group")] string WorkGroup,
[property: CliOption("--query-statement")] string QueryStatement
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}