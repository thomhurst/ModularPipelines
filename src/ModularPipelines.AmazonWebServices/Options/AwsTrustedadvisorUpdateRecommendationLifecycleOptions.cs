using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("trustedadvisor", "update-recommendation-lifecycle")]
public record AwsTrustedadvisorUpdateRecommendationLifecycleOptions(
[property: CommandSwitch("--lifecycle-stage")] string LifecycleStage,
[property: CommandSwitch("--recommendation-identifier")] string RecommendationIdentifier
) : AwsOptions
{
    [CommandSwitch("--update-reason")]
    public string? UpdateReason { get; set; }

    [CommandSwitch("--update-reason-code")]
    public string? UpdateReasonCode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}