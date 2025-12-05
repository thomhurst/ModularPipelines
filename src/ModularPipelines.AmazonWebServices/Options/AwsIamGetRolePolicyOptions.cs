using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "get-role-policy")]
public record AwsIamGetRolePolicyOptions(
[property: CliOption("--role-name")] string RoleName,
[property: CliOption("--policy-name")] string PolicyName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}