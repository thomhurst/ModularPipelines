using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "set-default-policy-version")]
public record AwsIamSetDefaultPolicyVersionOptions(
[property: CommandSwitch("--policy-arn")] string PolicyArn,
[property: CommandSwitch("--version-id")] string VersionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}