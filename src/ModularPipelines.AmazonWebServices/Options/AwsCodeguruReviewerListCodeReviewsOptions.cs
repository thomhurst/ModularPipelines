using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeguru-reviewer", "list-code-reviews")]
public record AwsCodeguruReviewerListCodeReviewsOptions(
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--provider-types")]
    public string[]? ProviderTypes { get; set; }

    [CliOption("--states")]
    public string[]? States { get; set; }

    [CliOption("--repository-names")]
    public string[]? RepositoryNames { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}