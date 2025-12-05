using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "revoke-client-vpn-ingress")]
public record AwsEc2RevokeClientVpnIngressOptions(
[property: CliOption("--client-vpn-endpoint-id")] string ClientVpnEndpointId,
[property: CliOption("--target-network-cidr")] string TargetNetworkCidr
) : AwsOptions
{
    [CliOption("--access-group-id")]
    public string? AccessGroupId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}