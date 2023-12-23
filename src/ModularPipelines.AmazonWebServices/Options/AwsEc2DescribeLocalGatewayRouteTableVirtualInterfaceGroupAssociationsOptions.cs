using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "describe-local-gateway-route-table-virtual-interface-group-associations")]
public record AwsEc2DescribeLocalGatewayRouteTableVirtualInterfaceGroupAssociationsOptions : AwsOptions
{
    [CommandSwitch("--local-gateway-route-table-virtual-interface-group-association-ids")]
    public string[]? LocalGatewayRouteTableVirtualInterfaceGroupAssociationIds { get; set; }

    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}