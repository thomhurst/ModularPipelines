using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "get-verified-access-group-policy")]
public record AwsEc2GetVerifiedAccessGroupPolicyOptions(
[property: CliOption("--verified-access-group-id")] string VerifiedAccessGroupId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}