using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-egress-only-internet-gateway")]
public record AwsEc2DeleteEgressOnlyInternetGatewayOptions(
[property: CliOption("--egress-only-internet-gateway-id")] string EgressOnlyInternetGatewayId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}