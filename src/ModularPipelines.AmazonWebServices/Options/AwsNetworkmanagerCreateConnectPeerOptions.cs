using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkmanager", "create-connect-peer")]
public record AwsNetworkmanagerCreateConnectPeerOptions(
[property: CommandSwitch("--connect-attachment-id")] string ConnectAttachmentId,
[property: CommandSwitch("--peer-address")] string PeerAddress
) : AwsOptions
{
    [CommandSwitch("--core-network-address")]
    public string? CoreNetworkAddress { get; set; }

    [CommandSwitch("--bgp-options")]
    public string? BgpOptions { get; set; }

    [CommandSwitch("--inside-cidr-blocks")]
    public string[]? InsideCidrBlocks { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--subnet-arn")]
    public string? SubnetArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}