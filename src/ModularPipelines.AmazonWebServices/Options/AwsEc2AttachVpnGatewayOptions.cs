using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "attach-vpn-gateway")]
public record AwsEc2AttachVpnGatewayOptions(
[property: CliOption("--vpc-id")] string VpcId,
[property: CliOption("--vpn-gateway-id")] string VpnGatewayId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}