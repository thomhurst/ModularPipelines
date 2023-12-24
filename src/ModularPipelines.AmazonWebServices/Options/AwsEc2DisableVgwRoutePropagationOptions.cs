using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "disable-vgw-route-propagation")]
public record AwsEc2DisableVgwRoutePropagationOptions(
[property: CommandSwitch("--gateway-id")] string GatewayId,
[property: CommandSwitch("--route-table-id")] string RouteTableId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}