using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-verified-access-endpoint")]
public record AwsEc2ModifyVerifiedAccessEndpointOptions(
[property: CommandSwitch("--verified-access-endpoint-id")] string VerifiedAccessEndpointId
) : AwsOptions
{
    [CommandSwitch("--verified-access-group-id")]
    public string? VerifiedAccessGroupId { get; set; }

    [CommandSwitch("--load-balancer-options")]
    public string? LoadBalancerOptions { get; set; }

    [CommandSwitch("--network-interface-options")]
    public string? NetworkInterfaceOptions { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}