using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "delete-user-policy")]
public record AwsIamDeleteUserPolicyOptions(
[property: CliOption("--user-name")] string UserName,
[property: CliOption("--policy-name")] string PolicyName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}