using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privatenetworks", "activate-network-site")]
public record AwsPrivatenetworksActivateNetworkSiteOptions(
[property: CliOption("--network-site-arn")] string NetworkSiteArn,
[property: CliOption("--shipping-address")] string ShippingAddress
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--commitment-configuration")]
    public string? CommitmentConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}