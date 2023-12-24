using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kendra", "submit-feedback")]
public record AwsKendraSubmitFeedbackOptions(
[property: CommandSwitch("--index-id")] string IndexId,
[property: CommandSwitch("--query-id")] string QueryId
) : AwsOptions
{
    [CommandSwitch("--click-feedback-items")]
    public string[]? ClickFeedbackItems { get; set; }

    [CommandSwitch("--relevance-feedback-items")]
    public string[]? RelevanceFeedbackItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}