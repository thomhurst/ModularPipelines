using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ce", "provide-anomaly-feedback")]
public record AwsCeProvideAnomalyFeedbackOptions(
[property: CommandSwitch("--anomaly-id")] string AnomalyId,
[property: CommandSwitch("--feedback")] string Feedback
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}