using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sesv2", "get-domain-deliverability-campaign")]
public record AwsSesv2GetDomainDeliverabilityCampaignOptions(
[property: CommandSwitch("--campaign-id")] string CampaignId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}