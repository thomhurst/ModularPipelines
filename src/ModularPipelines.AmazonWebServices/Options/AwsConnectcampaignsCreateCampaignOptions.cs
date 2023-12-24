using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectcampaigns", "create-campaign")]
public record AwsConnectcampaignsCreateCampaignOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--connect-instance-id")] string ConnectInstanceId,
[property: CommandSwitch("--dialer-config")] string DialerConfig,
[property: CommandSwitch("--outbound-call-config")] string OutboundCallConfig
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}