using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint-email", "get-domain-deliverability-campaign")]
public record AwsPinpointEmailGetDomainDeliverabilityCampaignOptions(
[property: CommandSwitch("--campaign-id")] string CampaignId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}