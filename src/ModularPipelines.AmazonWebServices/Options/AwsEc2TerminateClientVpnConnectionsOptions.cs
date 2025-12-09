using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "terminate-client-vpn-connections")]
public record AwsEc2TerminateClientVpnConnectionsOptions(
[property: CliOption("--client-vpn-endpoint-id")] string ClientVpnEndpointId
) : AwsOptions
{
    [CliOption("--connection-id")]
    public string? ConnectionId { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}