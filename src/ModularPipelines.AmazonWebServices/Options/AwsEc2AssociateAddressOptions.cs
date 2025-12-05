using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "associate-address")]
public record AwsEc2AssociateAddressOptions : AwsOptions
{
    [CliOption("--allocation-id")]
    public string? AllocationId { get; set; }

    [CliOption("--instance-id")]
    public string? InstanceId { get; set; }

    [CliOption("--public-ip")]
    public string? PublicIp { get; set; }

    [CliOption("--network-interface-id")]
    public string? NetworkInterfaceId { get; set; }

    [CliOption("--private-ip-address")]
    public string? PrivateIpAddress { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}