using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resiliencehub", "list-recommendation-templates")]
public record AwsResiliencehubListRecommendationTemplatesOptions(
[property: CliOption("--assessment-arn")] string AssessmentArn
) : AwsOptions
{
    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--recommendation-template-arn")]
    public string? RecommendationTemplateArn { get; set; }

    [CliOption("--status")]
    public string[]? Status { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}