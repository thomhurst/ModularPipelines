using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptunedata", "execute-gremlin-explain-query")]
public record AwsNeptunedataExecuteGremlinExplainQueryOptions(
[property: CommandSwitch("--gremlin-query")] string GremlinQuery
) : AwsOptions;