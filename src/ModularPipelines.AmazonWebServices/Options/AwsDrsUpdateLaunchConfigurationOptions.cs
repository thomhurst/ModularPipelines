using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("drs", "update-launch-configuration")]
public record AwsDrsUpdateLaunchConfigurationOptions(
[property: CommandSwitch("--source-server-id")] string SourceServerId
) : AwsOptions
{
    [CommandSwitch("--launch-disposition")]
    public string? LaunchDisposition { get; set; }

    [CommandSwitch("--launch-into-instance-properties")]
    public string? LaunchIntoInstanceProperties { get; set; }

    [CommandSwitch("--licensing")]
    public string? Licensing { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--target-instance-type-right-sizing-method")]
    public string? TargetInstanceTypeRightSizingMethod { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}