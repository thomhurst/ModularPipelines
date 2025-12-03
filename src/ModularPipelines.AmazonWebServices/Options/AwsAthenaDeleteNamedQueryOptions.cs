using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("athena", "delete-named-query")]
public record AwsAthenaDeleteNamedQueryOptions : AwsOptions
{
    [CliOption("--named-query-id")]
    public string? NamedQueryId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}