using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kendra", "submit-feedback")]
public record AwsKendraSubmitFeedbackOptions(
[property: CliOption("--index-id")] string IndexId,
[property: CliOption("--query-id")] string QueryId
) : AwsOptions
{
    [CliOption("--click-feedback-items")]
    public string[]? ClickFeedbackItems { get; set; }

    [CliOption("--relevance-feedback-items")]
    public string[]? RelevanceFeedbackItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}