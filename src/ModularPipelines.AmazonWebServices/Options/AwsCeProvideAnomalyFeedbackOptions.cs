using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ce", "provide-anomaly-feedback")]
public record AwsCeProvideAnomalyFeedbackOptions(
[property: CliOption("--anomaly-id")] string AnomalyId,
[property: CliOption("--feedback")] string Feedback
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}