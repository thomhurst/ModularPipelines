using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptunedata", "execute-gremlin-query")]
public record AwsNeptunedataExecuteGremlinQueryOptions(
[property: CliOption("--gremlin-query")] string GremlinQuery
) : AwsOptions
{
    [CliOption("--serializer")]
    public string? Serializer { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}