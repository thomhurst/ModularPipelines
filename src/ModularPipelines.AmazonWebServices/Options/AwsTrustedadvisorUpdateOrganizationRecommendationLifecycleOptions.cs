using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("trustedadvisor", "update-organization-recommendation-lifecycle")]
public record AwsTrustedadvisorUpdateOrganizationRecommendationLifecycleOptions(
[property: CommandSwitch("--lifecycle-stage")] string LifecycleStage,
[property: CommandSwitch("--organization-recommendation-identifier")] string OrganizationRecommendationIdentifier
) : AwsOptions
{
    [CommandSwitch("--update-reason")]
    public string? UpdateReason { get; set; }

    [CommandSwitch("--update-reason-code")]
    public string? UpdateReasonCode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}