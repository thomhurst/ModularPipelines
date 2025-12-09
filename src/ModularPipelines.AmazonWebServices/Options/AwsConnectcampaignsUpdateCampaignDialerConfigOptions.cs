using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectcampaigns", "update-campaign-dialer-config")]
public record AwsConnectcampaignsUpdateCampaignDialerConfigOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--dialer-config")] string DialerConfig
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}