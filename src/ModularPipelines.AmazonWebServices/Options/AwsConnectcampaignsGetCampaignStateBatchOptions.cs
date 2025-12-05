using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectcampaigns", "get-campaign-state-batch")]
public record AwsConnectcampaignsGetCampaignStateBatchOptions(
[property: CliOption("--campaign-ids")] string[] CampaignIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}