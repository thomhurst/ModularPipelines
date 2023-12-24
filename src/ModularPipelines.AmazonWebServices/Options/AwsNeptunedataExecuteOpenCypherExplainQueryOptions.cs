using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptunedata", "execute-open-cypher-explain-query")]
public record AwsNeptunedataExecuteOpenCypherExplainQueryOptions(
[property: CommandSwitch("--open-cypher-query")] string OpenCypherQuery,
[property: CommandSwitch("--explain-mode")] string ExplainMode
) : AwsOptions
{
    [CommandSwitch("--parameters")]
    public string? Parameters { get; set; }
}