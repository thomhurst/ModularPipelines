using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mgn", "update-launch-configuration-template")]
public record AwsMgnUpdateLaunchConfigurationTemplateOptions(
[property: CommandSwitch("--launch-configuration-template-id")] string LaunchConfigurationTemplateId
) : AwsOptions
{
    [CommandSwitch("--boot-mode")]
    public string? BootMode { get; set; }

    [CommandSwitch("--large-volume-conf")]
    public string? LargeVolumeConf { get; set; }

    [CommandSwitch("--launch-disposition")]
    public string? LaunchDisposition { get; set; }

    [CommandSwitch("--licensing")]
    public string? Licensing { get; set; }

    [CommandSwitch("--map-auto-tagging-mpe-id")]
    public string? MapAutoTaggingMpeId { get; set; }

    [CommandSwitch("--post-launch-actions")]
    public string? PostLaunchActions { get; set; }

    [CommandSwitch("--small-volume-conf")]
    public string? SmallVolumeConf { get; set; }

    [CommandSwitch("--small-volume-max-size")]
    public long? SmallVolumeMaxSize { get; set; }

    [CommandSwitch("--target-instance-type-right-sizing-method")]
    public string? TargetInstanceTypeRightSizingMethod { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}