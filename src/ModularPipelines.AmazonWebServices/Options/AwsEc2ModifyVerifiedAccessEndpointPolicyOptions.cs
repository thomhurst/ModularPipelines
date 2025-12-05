using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-verified-access-endpoint-policy")]
public record AwsEc2ModifyVerifiedAccessEndpointPolicyOptions(
[property: CliOption("--verified-access-endpoint-id")] string VerifiedAccessEndpointId
) : AwsOptions
{
    [CliOption("--policy-document")]
    public string? PolicyDocument { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--sse-specification")]
    public string? SseSpecification { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}