using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "get-campaign-versions")]
public record AwsPinpointGetCampaignVersionsOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--campaign-id")] string CampaignId
) : AwsOptions
{
    [CommandSwitch("--page-size")]
    public string? PageSize { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}