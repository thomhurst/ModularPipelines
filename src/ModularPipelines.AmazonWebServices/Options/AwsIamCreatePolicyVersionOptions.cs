using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "create-policy-version")]
public record AwsIamCreatePolicyVersionOptions(
[property: CommandSwitch("--policy-arn")] string PolicyArn,
[property: CommandSwitch("--policy-document")] string PolicyDocument
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}