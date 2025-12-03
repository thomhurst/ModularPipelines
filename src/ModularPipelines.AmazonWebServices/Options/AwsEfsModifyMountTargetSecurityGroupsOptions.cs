using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("efs", "modify-mount-target-security-groups")]
public record AwsEfsModifyMountTargetSecurityGroupsOptions(
[property: CliOption("--mount-target-id")] string MountTargetId
) : AwsOptions
{
    [CliOption("--security-groups")]
    public string[]? SecurityGroups { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}