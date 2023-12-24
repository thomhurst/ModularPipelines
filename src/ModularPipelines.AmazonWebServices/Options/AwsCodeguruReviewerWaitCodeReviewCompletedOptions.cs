using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codeguru-reviewer", "wait", "code-review-completed")]
public record AwsCodeguruReviewerWaitCodeReviewCompletedOptions(
[property: CommandSwitch("--code-review-arn")] string CodeReviewArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}