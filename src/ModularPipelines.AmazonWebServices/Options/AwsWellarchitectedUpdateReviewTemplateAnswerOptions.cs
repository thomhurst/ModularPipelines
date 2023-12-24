using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "update-review-template-answer")]
public record AwsWellarchitectedUpdateReviewTemplateAnswerOptions(
[property: CommandSwitch("--template-arn")] string TemplateArn,
[property: CommandSwitch("--lens-alias")] string LensAlias,
[property: CommandSwitch("--question-id")] string QuestionId
) : AwsOptions
{
    [CommandSwitch("--selected-choices")]
    public string[]? SelectedChoices { get; set; }

    [CommandSwitch("--choice-updates")]
    public IEnumerable<KeyValue>? ChoiceUpdates { get; set; }

    [CommandSwitch("--notes")]
    public string? Notes { get; set; }

    [CommandSwitch("--reason")]
    public string? Reason { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}