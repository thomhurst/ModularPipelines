using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medialive", "reboot-input-device")]
public record AwsMedialiveRebootInputDeviceOptions(
[property: CommandSwitch("--input-device-id")] string InputDeviceId
) : AwsOptions
{
    [CommandSwitch("--force")]
    public string? Force { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}