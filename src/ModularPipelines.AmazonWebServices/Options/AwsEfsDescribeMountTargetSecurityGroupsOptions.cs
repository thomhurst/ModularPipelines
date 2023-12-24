using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("efs", "describe-mount-target-security-groups")]
public record AwsEfsDescribeMountTargetSecurityGroupsOptions(
[property: CommandSwitch("--mount-target-id")] string MountTargetId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}