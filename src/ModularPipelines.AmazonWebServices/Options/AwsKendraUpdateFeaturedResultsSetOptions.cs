using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kendra", "update-featured-results-set")]
public record AwsKendraUpdateFeaturedResultsSetOptions(
[property: CliOption("--index-id")] string IndexId,
[property: CliOption("--featured-results-set-id")] string FeaturedResultsSetId
) : AwsOptions
{
    [CliOption("--featured-results-set-name")]
    public string? FeaturedResultsSetName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--query-texts")]
    public string[]? QueryTexts { get; set; }

    [CliOption("--featured-documents")]
    public string[]? FeaturedDocuments { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}