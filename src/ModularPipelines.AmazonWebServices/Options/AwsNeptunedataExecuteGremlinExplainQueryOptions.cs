using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptunedata", "execute-gremlin-explain-query")]
public record AwsNeptunedataExecuteGremlinExplainQueryOptions(
[property: CliOption("--gremlin-query")] string GremlinQuery
) : AwsOptions;