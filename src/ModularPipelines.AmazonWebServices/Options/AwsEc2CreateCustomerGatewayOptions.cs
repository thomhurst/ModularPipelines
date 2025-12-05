using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-customer-gateway")]
public record AwsEc2CreateCustomerGatewayOptions(
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--bgp-asn")]
    public int? BgpAsn { get; set; }

    [CliOption("--public-ip")]
    public string? PublicIp { get; set; }

    [CliOption("--certificate-arn")]
    public string? CertificateArn { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--device-name")]
    public string? DeviceName { get; set; }

    [CliOption("--ip-address")]
    public string? IpAddress { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}