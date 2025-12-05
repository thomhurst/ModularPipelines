using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "detach-role-policy")]
public record AwsIamDetachRolePolicyOptions(
[property: CliOption("--role-name")] string RoleName,
[property: CliOption("--policy-arn")] string PolicyArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}