using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "get-review-template-answer")]
public record AwsWellarchitectedGetReviewTemplateAnswerOptions(
[property: CliOption("--template-arn")] string TemplateArn,
[property: CliOption("--lens-alias")] string LensAlias,
[property: CliOption("--question-id")] string QuestionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}