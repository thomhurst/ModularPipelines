using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("efs", "modify-mount-target-security-groups")]
public record AwsEfsModifyMountTargetSecurityGroupsOptions(
[property: CommandSwitch("--mount-target-id")] string MountTargetId
) : AwsOptions
{
    [CommandSwitch("--security-groups")]
    public string[]? SecurityGroups { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}