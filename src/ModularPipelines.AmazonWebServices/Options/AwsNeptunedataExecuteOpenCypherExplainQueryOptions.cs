using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptunedata", "execute-open-cypher-explain-query")]
public record AwsNeptunedataExecuteOpenCypherExplainQueryOptions(
[property: CliOption("--open-cypher-query")] string OpenCypherQuery,
[property: CliOption("--explain-mode")] string ExplainMode
) : AwsOptions
{
    [CliOption("--parameters")]
    public string? Parameters { get; set; }
}