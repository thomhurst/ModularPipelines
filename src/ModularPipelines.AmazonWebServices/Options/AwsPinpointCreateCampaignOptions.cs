using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "create-campaign")]
public record AwsPinpointCreateCampaignOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--write-campaign-request")] string WriteCampaignRequest
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}