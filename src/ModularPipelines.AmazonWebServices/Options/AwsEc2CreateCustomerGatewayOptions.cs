using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-customer-gateway")]
public record AwsEc2CreateCustomerGatewayOptions(
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--bgp-asn")]
    public int? BgpAsn { get; set; }

    [CommandSwitch("--public-ip")]
    public string? PublicIp { get; set; }

    [CommandSwitch("--certificate-arn")]
    public string? CertificateArn { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--device-name")]
    public string? DeviceName { get; set; }

    [CommandSwitch("--ip-address")]
    public string? IpAddress { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}