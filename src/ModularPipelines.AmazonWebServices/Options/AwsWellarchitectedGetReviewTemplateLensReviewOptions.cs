using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "get-review-template-lens-review")]
public record AwsWellarchitectedGetReviewTemplateLensReviewOptions(
[property: CliOption("--template-arn")] string TemplateArn,
[property: CliOption("--lens-alias")] string LensAlias
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}