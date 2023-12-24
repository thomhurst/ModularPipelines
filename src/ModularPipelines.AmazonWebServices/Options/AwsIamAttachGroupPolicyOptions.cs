using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "attach-group-policy")]
public record AwsIamAttachGroupPolicyOptions(
[property: CommandSwitch("--group-name")] string GroupName,
[property: CommandSwitch("--policy-arn")] string PolicyArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}