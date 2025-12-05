using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectcampaigns", "create-campaign")]
public record AwsConnectcampaignsCreateCampaignOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--connect-instance-id")] string ConnectInstanceId,
[property: CliOption("--dialer-config")] string DialerConfig,
[property: CliOption("--outbound-call-config")] string OutboundCallConfig
) : AwsOptions
{
    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}