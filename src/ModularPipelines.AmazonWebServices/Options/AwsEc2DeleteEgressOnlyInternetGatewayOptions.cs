using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "delete-egress-only-internet-gateway")]
public record AwsEc2DeleteEgressOnlyInternetGatewayOptions(
[property: CommandSwitch("--egress-only-internet-gateway-id")] string EgressOnlyInternetGatewayId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}