using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "get-review-template-answer")]
public record AwsWellarchitectedGetReviewTemplateAnswerOptions(
[property: CommandSwitch("--template-arn")] string TemplateArn,
[property: CommandSwitch("--lens-alias")] string LensAlias,
[property: CommandSwitch("--question-id")] string QuestionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}