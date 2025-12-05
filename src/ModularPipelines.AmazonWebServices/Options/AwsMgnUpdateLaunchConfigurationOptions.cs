using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mgn", "update-launch-configuration")]
public record AwsMgnUpdateLaunchConfigurationOptions(
[property: CliOption("--source-server-id")] string SourceServerId
) : AwsOptions
{
    [CliOption("--account-id")]
    public string? AccountId { get; set; }

    [CliOption("--boot-mode")]
    public string? BootMode { get; set; }

    [CliOption("--launch-disposition")]
    public string? LaunchDisposition { get; set; }

    [CliOption("--licensing")]
    public string? Licensing { get; set; }

    [CliOption("--map-auto-tagging-mpe-id")]
    public string? MapAutoTaggingMpeId { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--post-launch-actions")]
    public string? PostLaunchActions { get; set; }

    [CliOption("--target-instance-type-right-sizing-method")]
    public string? TargetInstanceTypeRightSizingMethod { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}