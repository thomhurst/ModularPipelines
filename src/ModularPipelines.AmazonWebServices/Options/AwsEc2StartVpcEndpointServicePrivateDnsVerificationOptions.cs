using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "start-vpc-endpoint-service-private-dns-verification")]
public record AwsEc2StartVpcEndpointServicePrivateDnsVerificationOptions(
[property: CliOption("--service-id")] string ServiceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}