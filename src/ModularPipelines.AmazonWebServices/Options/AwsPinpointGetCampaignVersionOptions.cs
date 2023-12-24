using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "get-campaign-version")]
public record AwsPinpointGetCampaignVersionOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--campaign-id")] string CampaignId,
[property: CommandSwitch("--campaign-version")] string CampaignVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}