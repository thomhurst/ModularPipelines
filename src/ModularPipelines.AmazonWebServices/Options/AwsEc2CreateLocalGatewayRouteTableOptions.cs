using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-local-gateway-route-table")]
public record AwsEc2CreateLocalGatewayRouteTableOptions(
[property: CommandSwitch("--local-gateway-id")] string LocalGatewayId
) : AwsOptions
{
    [CommandSwitch("--mode")]
    public string? Mode { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}