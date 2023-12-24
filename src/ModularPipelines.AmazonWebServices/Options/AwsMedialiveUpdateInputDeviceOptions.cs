using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medialive", "update-input-device")]
public record AwsMedialiveUpdateInputDeviceOptions(
[property: CommandSwitch("--input-device-id")] string InputDeviceId
) : AwsOptions
{
    [CommandSwitch("--hd-device-settings")]
    public string? HdDeviceSettings { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--uhd-device-settings")]
    public string? UhdDeviceSettings { get; set; }

    [CommandSwitch("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}