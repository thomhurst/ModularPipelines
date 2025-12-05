using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("efs", "create-mount-target")]
public record AwsEfsCreateMountTargetOptions(
[property: CliOption("--file-system-id")] string FileSystemId,
[property: CliOption("--subnet-id")] string SubnetId
) : AwsOptions
{
    [CliOption("--ip-address")]
    public string? IpAddress { get; set; }

    [CliOption("--security-groups")]
    public string[]? SecurityGroups { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}