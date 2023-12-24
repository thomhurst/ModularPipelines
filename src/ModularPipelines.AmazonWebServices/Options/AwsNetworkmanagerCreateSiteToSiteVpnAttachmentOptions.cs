using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkmanager", "create-site-to-site-vpn-attachment")]
public record AwsNetworkmanagerCreateSiteToSiteVpnAttachmentOptions(
[property: CommandSwitch("--core-network-id")] string CoreNetworkId,
[property: CommandSwitch("--vpn-connection-arn")] string VpnConnectionArn
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}