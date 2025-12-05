using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("trustedadvisor", "update-organization-recommendation-lifecycle")]
public record AwsTrustedadvisorUpdateOrganizationRecommendationLifecycleOptions(
[property: CliOption("--lifecycle-stage")] string LifecycleStage,
[property: CliOption("--organization-recommendation-identifier")] string OrganizationRecommendationIdentifier
) : AwsOptions
{
    [CliOption("--update-reason")]
    public string? UpdateReason { get; set; }

    [CliOption("--update-reason-code")]
    public string? UpdateReasonCode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}