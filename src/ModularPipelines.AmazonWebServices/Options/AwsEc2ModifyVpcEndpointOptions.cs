using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-vpc-endpoint")]
public record AwsEc2ModifyVpcEndpointOptions(
[property: CliOption("--vpc-endpoint-id")] string VpcEndpointId
) : AwsOptions
{
    [CliOption("--policy-document")]
    public string? PolicyDocument { get; set; }

    [CliOption("--add-route-table-ids")]
    public string[]? AddRouteTableIds { get; set; }

    [CliOption("--remove-route-table-ids")]
    public string[]? RemoveRouteTableIds { get; set; }

    [CliOption("--add-subnet-ids")]
    public string[]? AddSubnetIds { get; set; }

    [CliOption("--remove-subnet-ids")]
    public string[]? RemoveSubnetIds { get; set; }

    [CliOption("--add-security-group-ids")]
    public string[]? AddSecurityGroupIds { get; set; }

    [CliOption("--remove-security-group-ids")]
    public string[]? RemoveSecurityGroupIds { get; set; }

    [CliOption("--ip-address-type")]
    public string? IpAddressType { get; set; }

    [CliOption("--dns-options")]
    public string? DnsOptions { get; set; }

    [CliOption("--subnet-configurations")]
    public string[]? SubnetConfigurations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}