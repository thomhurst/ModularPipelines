using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mgn", "update-launch-configuration")]
public record AwsMgnUpdateLaunchConfigurationOptions(
[property: CommandSwitch("--source-server-id")] string SourceServerId
) : AwsOptions
{
    [CommandSwitch("--account-id")]
    public string? AccountId { get; set; }

    [CommandSwitch("--boot-mode")]
    public string? BootMode { get; set; }

    [CommandSwitch("--launch-disposition")]
    public string? LaunchDisposition { get; set; }

    [CommandSwitch("--licensing")]
    public string? Licensing { get; set; }

    [CommandSwitch("--map-auto-tagging-mpe-id")]
    public string? MapAutoTaggingMpeId { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--post-launch-actions")]
    public string? PostLaunchActions { get; set; }

    [CommandSwitch("--target-instance-type-right-sizing-method")]
    public string? TargetInstanceTypeRightSizingMethod { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}