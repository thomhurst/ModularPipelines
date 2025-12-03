using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kendra", "batch-delete-featured-results-set")]
public record AwsKendraBatchDeleteFeaturedResultsSetOptions(
[property: CliOption("--index-id")] string IndexId,
[property: CliOption("--featured-results-set-ids")] string[] FeaturedResultsSetIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}