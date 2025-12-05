using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptunedata", "execute-gremlin-profile-query")]
public record AwsNeptunedataExecuteGremlinProfileQueryOptions(
[property: CliOption("--gremlin-query")] string GremlinQuery
) : AwsOptions
{
    [CliOption("--chop")]
    public int? Chop { get; set; }

    [CliOption("--serializer")]
    public string? Serializer { get; set; }
}