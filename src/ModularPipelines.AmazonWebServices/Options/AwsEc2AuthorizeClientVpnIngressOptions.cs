using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "authorize-client-vpn-ingress")]
public record AwsEc2AuthorizeClientVpnIngressOptions(
[property: CliOption("--client-vpn-endpoint-id")] string ClientVpnEndpointId,
[property: CliOption("--target-network-cidr")] string TargetNetworkCidr
) : AwsOptions
{
    [CliOption("--access-group-id")]
    public string? AccessGroupId { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}