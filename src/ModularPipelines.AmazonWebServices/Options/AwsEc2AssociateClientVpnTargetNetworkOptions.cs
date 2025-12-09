using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "associate-client-vpn-target-network")]
public record AwsEc2AssociateClientVpnTargetNetworkOptions(
[property: CliOption("--client-vpn-endpoint-id")] string ClientVpnEndpointId,
[property: CliOption("--subnet-id")] string SubnetId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}