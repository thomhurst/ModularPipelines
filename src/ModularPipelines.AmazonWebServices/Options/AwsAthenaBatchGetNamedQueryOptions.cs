using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("athena", "batch-get-named-query")]
public record AwsAthenaBatchGetNamedQueryOptions(
[property: CliOption("--named-query-ids")] string[] NamedQueryIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}