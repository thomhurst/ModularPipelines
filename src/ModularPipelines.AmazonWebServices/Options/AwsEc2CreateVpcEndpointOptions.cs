using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-vpc-endpoint")]
public record AwsEc2CreateVpcEndpointOptions(
[property: CliOption("--vpc-id")] string VpcId,
[property: CliOption("--service-name")] string ServiceName
) : AwsOptions
{
    [CliOption("--vpc-endpoint-type")]
    public string? VpcEndpointType { get; set; }

    [CliOption("--policy-document")]
    public string? PolicyDocument { get; set; }

    [CliOption("--route-table-ids")]
    public string[]? RouteTableIds { get; set; }

    [CliOption("--subnet-ids")]
    public string[]? SubnetIds { get; set; }

    [CliOption("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CliOption("--ip-address-type")]
    public string? IpAddressType { get; set; }

    [CliOption("--dns-options")]
    public string? DnsOptions { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--subnet-configurations")]
    public string[]? SubnetConfigurations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}