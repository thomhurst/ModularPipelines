using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "list-review-template-answers")]
public record AwsWellarchitectedListReviewTemplateAnswersOptions(
[property: CliOption("--template-arn")] string TemplateArn,
[property: CliOption("--lens-alias")] string LensAlias
) : AwsOptions
{
    [CliOption("--pillar-id")]
    public string? PillarId { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}