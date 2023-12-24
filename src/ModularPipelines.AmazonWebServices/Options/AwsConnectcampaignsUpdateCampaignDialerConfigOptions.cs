using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectcampaigns", "update-campaign-dialer-config")]
public record AwsConnectcampaignsUpdateCampaignDialerConfigOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--dialer-config")] string DialerConfig
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}