using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-verified-access-endpoint")]
public record AwsEc2ModifyVerifiedAccessEndpointOptions(
[property: CliOption("--verified-access-endpoint-id")] string VerifiedAccessEndpointId
) : AwsOptions
{
    [CliOption("--verified-access-group-id")]
    public string? VerifiedAccessGroupId { get; set; }

    [CliOption("--load-balancer-options")]
    public string? LoadBalancerOptions { get; set; }

    [CliOption("--network-interface-options")]
    public string? NetworkInterfaceOptions { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}