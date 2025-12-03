using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "get-verified-access-endpoint-policy")]
public record AwsEc2GetVerifiedAccessEndpointPolicyOptions(
[property: CliOption("--verified-access-endpoint-id")] string VerifiedAccessEndpointId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}