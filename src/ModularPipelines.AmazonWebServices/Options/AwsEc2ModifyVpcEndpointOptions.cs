using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-vpc-endpoint")]
public record AwsEc2ModifyVpcEndpointOptions(
[property: CommandSwitch("--vpc-endpoint-id")] string VpcEndpointId
) : AwsOptions
{
    [CommandSwitch("--policy-document")]
    public string? PolicyDocument { get; set; }

    [CommandSwitch("--add-route-table-ids")]
    public string[]? AddRouteTableIds { get; set; }

    [CommandSwitch("--remove-route-table-ids")]
    public string[]? RemoveRouteTableIds { get; set; }

    [CommandSwitch("--add-subnet-ids")]
    public string[]? AddSubnetIds { get; set; }

    [CommandSwitch("--remove-subnet-ids")]
    public string[]? RemoveSubnetIds { get; set; }

    [CommandSwitch("--add-security-group-ids")]
    public string[]? AddSecurityGroupIds { get; set; }

    [CommandSwitch("--remove-security-group-ids")]
    public string[]? RemoveSecurityGroupIds { get; set; }

    [CommandSwitch("--ip-address-type")]
    public string? IpAddressType { get; set; }

    [CommandSwitch("--dns-options")]
    public string? DnsOptions { get; set; }

    [CommandSwitch("--subnet-configurations")]
    public string[]? SubnetConfigurations { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}