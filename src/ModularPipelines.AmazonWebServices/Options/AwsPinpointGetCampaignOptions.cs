using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "get-campaign")]
public record AwsPinpointGetCampaignOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--campaign-id")] string CampaignId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}