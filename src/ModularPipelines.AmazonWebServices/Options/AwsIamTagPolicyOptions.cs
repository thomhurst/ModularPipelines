using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "tag-policy")]
public record AwsIamTagPolicyOptions(
[property: CliOption("--policy-arn")] string PolicyArn,
[property: CliOption("--tags")] string[] Tags
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}