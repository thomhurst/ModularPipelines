using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kendra", "create-featured-results-set")]
public record AwsKendraCreateFeaturedResultsSetOptions(
[property: CliOption("--index-id")] string IndexId,
[property: CliOption("--featured-results-set-name")] string FeaturedResultsSetName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--query-texts")]
    public string[]? QueryTexts { get; set; }

    [CliOption("--featured-documents")]
    public string[]? FeaturedDocuments { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}