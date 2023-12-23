using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "delete-policy-version")]
public record AwsIamDeletePolicyVersionOptions(
[property: CommandSwitch("--policy-arn")] string PolicyArn,
[property: CommandSwitch("--version-id")] string VersionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}