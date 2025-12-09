using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mgn", "update-launch-configuration-template")]
public record AwsMgnUpdateLaunchConfigurationTemplateOptions(
[property: CliOption("--launch-configuration-template-id")] string LaunchConfigurationTemplateId
) : AwsOptions
{
    [CliOption("--boot-mode")]
    public string? BootMode { get; set; }

    [CliOption("--large-volume-conf")]
    public string? LargeVolumeConf { get; set; }

    [CliOption("--launch-disposition")]
    public string? LaunchDisposition { get; set; }

    [CliOption("--licensing")]
    public string? Licensing { get; set; }

    [CliOption("--map-auto-tagging-mpe-id")]
    public string? MapAutoTaggingMpeId { get; set; }

    [CliOption("--post-launch-actions")]
    public string? PostLaunchActions { get; set; }

    [CliOption("--small-volume-conf")]
    public string? SmallVolumeConf { get; set; }

    [CliOption("--small-volume-max-size")]
    public long? SmallVolumeMaxSize { get; set; }

    [CliOption("--target-instance-type-right-sizing-method")]
    public string? TargetInstanceTypeRightSizingMethod { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}