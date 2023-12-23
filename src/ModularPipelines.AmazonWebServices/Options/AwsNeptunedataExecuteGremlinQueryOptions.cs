using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptunedata", "execute-gremlin-query")]
public record AwsNeptunedataExecuteGremlinQueryOptions(
[property: CommandSwitch("--gremlin-query")] string GremlinQuery
) : AwsOptions
{
    [CommandSwitch("--serializer")]
    public string? Serializer { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}