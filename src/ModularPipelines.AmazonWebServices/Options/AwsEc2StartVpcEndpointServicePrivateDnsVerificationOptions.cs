using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "start-vpc-endpoint-service-private-dns-verification")]
public record AwsEc2StartVpcEndpointServicePrivateDnsVerificationOptions(
[property: CommandSwitch("--service-id")] string ServiceId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}