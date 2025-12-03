using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elbv2", "create-load-balancer")]
public record AwsElbv2CreateLoadBalancerOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--subnets")]
    public string[]? Subnets { get; set; }

    [CliOption("--subnet-mappings")]
    public string[]? SubnetMappings { get; set; }

    [CliOption("--security-groups")]
    public string[]? SecurityGroups { get; set; }

    [CliOption("--scheme")]
    public string? Scheme { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--ip-address-type")]
    public string? IpAddressType { get; set; }

    [CliOption("--customer-owned-ipv4-pool")]
    public string? CustomerOwnedIpv4Pool { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}