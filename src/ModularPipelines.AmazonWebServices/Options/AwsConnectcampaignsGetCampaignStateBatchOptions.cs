using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectcampaigns", "get-campaign-state-batch")]
public record AwsConnectcampaignsGetCampaignStateBatchOptions(
[property: CommandSwitch("--campaign-ids")] string[] CampaignIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}