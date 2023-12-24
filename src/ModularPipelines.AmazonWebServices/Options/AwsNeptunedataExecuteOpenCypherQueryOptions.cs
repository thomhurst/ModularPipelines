using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptunedata", "execute-open-cypher-query")]
public record AwsNeptunedataExecuteOpenCypherQueryOptions(
[property: CommandSwitch("--open-cypher-query")] string OpenCypherQuery
) : AwsOptions
{
    [CommandSwitch("--parameters")]
    public string? Parameters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}