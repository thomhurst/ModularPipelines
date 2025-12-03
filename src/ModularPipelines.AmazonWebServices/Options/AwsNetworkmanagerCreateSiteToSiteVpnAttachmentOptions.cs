using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "create-site-to-site-vpn-attachment")]
public record AwsNetworkmanagerCreateSiteToSiteVpnAttachmentOptions(
[property: CliOption("--core-network-id")] string CoreNetworkId,
[property: CliOption("--vpn-connection-arn")] string VpnConnectionArn
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}