using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("location", "get-device-position")]
public record AwsLocationGetDevicePositionOptions(
[property: CommandSwitch("--device-id")] string DeviceId,
[property: CommandSwitch("--tracker-name")] string TrackerName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}