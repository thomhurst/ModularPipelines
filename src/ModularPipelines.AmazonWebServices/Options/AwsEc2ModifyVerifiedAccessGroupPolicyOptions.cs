using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-verified-access-group-policy")]
public record AwsEc2ModifyVerifiedAccessGroupPolicyOptions(
[property: CliOption("--verified-access-group-id")] string VerifiedAccessGroupId
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