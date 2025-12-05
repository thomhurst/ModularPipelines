using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptunedata", "execute-open-cypher-query")]
public record AwsNeptunedataExecuteOpenCypherQueryOptions(
[property: CliOption("--open-cypher-query")] string OpenCypherQuery
) : AwsOptions
{
    [CliOption("--parameters")]
    public string? Parameters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}