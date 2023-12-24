using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "update-campaign")]
public record AwsPinpointUpdateCampaignOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--campaign-id")] string CampaignId,
[property: CommandSwitch("--write-campaign-request")] string WriteCampaignRequest
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}