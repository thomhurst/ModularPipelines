using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medialive", "start-input-device-maintenance-window")]
public record AwsMedialiveStartInputDeviceMaintenanceWindowOptions(
[property: CommandSwitch("--input-device-id")] string InputDeviceId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}