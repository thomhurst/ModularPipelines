using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privatenetworks", "activate-network-site")]
public record AwsPrivatenetworksActivateNetworkSiteOptions(
[property: CommandSwitch("--network-site-arn")] string NetworkSiteArn,
[property: CommandSwitch("--shipping-address")] string ShippingAddress
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--commitment-configuration")]
    public string? CommitmentConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}