using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("athena", "update-named-query")]
public record AwsAthenaUpdateNamedQueryOptions(
[property: CliOption("--named-query-id")] string NamedQueryId,
[property: CliOption("--name")] string Name,
[property: CliOption("--query-string")] string QueryString
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}