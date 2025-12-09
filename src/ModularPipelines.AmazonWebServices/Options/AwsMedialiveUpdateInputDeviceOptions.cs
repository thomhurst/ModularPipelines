using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("medialive", "update-input-device")]
public record AwsMedialiveUpdateInputDeviceOptions(
[property: CliOption("--input-device-id")] string InputDeviceId
) : AwsOptions
{
    [CliOption("--hd-device-settings")]
    public string? HdDeviceSettings { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--uhd-device-settings")]
    public string? UhdDeviceSettings { get; set; }

    [CliOption("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}