using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("medialive", "reboot-input-device")]
public record AwsMedialiveRebootInputDeviceOptions(
[property: CliOption("--input-device-id")] string InputDeviceId
) : AwsOptions
{
    [CliOption("--force")]
    public string? Force { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}