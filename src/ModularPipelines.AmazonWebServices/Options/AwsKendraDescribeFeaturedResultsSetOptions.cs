using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kendra", "describe-featured-results-set")]
public record AwsKendraDescribeFeaturedResultsSetOptions(
[property: CliOption("--index-id")] string IndexId,
[property: CliOption("--featured-results-set-id")] string FeaturedResultsSetId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}