using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "delete-policy-version")]
public record AwsIamDeletePolicyVersionOptions(
[property: CliOption("--policy-arn")] string PolicyArn,
[property: CliOption("--version-id")] string VersionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}