using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "create-connect-peer")]
public record AwsNetworkmanagerCreateConnectPeerOptions(
[property: CliOption("--connect-attachment-id")] string ConnectAttachmentId,
[property: CliOption("--peer-address")] string PeerAddress
) : AwsOptions
{
    [CliOption("--core-network-address")]
    public string? CoreNetworkAddress { get; set; }

    [CliOption("--bgp-options")]
    public string? BgpOptions { get; set; }

    [CliOption("--inside-cidr-blocks")]
    public string[]? InsideCidrBlocks { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--subnet-arn")]
    public string? SubnetArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}