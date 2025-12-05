using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-subnet-attribute")]
public record AwsEc2ModifySubnetAttributeOptions(
[property: CliOption("--subnet-id")] string SubnetId
) : AwsOptions
{
    [CliOption("--customer-owned-ipv4-pool")]
    public string? CustomerOwnedIpv4Pool { get; set; }

    [CliOption("--private-dns-hostname-type-on-launch")]
    public string? PrivateDnsHostnameTypeOnLaunch { get; set; }

    [CliOption("--enable-lni-at-device-index")]
    public int? EnableLniAtDeviceIndex { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}