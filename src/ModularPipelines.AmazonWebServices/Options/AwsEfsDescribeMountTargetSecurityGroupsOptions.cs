using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("efs", "describe-mount-target-security-groups")]
public record AwsEfsDescribeMountTargetSecurityGroupsOptions(
[property: CliOption("--mount-target-id")] string MountTargetId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}