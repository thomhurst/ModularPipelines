using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "attach-user-policy")]
public record AwsIamAttachUserPolicyOptions(
[property: CliOption("--user-name")] string UserName,
[property: CliOption("--policy-arn")] string PolicyArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}