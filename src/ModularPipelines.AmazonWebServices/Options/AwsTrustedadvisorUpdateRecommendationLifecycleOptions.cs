using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("trustedadvisor", "update-recommendation-lifecycle")]
public record AwsTrustedadvisorUpdateRecommendationLifecycleOptions(
[property: CliOption("--lifecycle-stage")] string LifecycleStage,
[property: CliOption("--recommendation-identifier")] string RecommendationIdentifier
) : AwsOptions
{
    [CliOption("--update-reason")]
    public string? UpdateReason { get; set; }

    [CliOption("--update-reason-code")]
    public string? UpdateReasonCode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}