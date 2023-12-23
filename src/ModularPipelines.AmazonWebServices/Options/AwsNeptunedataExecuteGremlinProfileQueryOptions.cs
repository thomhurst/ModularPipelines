using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptunedata", "execute-gremlin-profile-query")]
public record AwsNeptunedataExecuteGremlinProfileQueryOptions(
[property: CommandSwitch("--gremlin-query")] string GremlinQuery
) : AwsOptions
{
    [CommandSwitch("--chop")]
    public int? Chop { get; set; }

    [CommandSwitch("--serializer")]
    public string? Serializer { get; set; }
}