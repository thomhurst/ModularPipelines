using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "start-single-wireless-device-import-task")]
public record AwsIotwirelessStartSingleWirelessDeviceImportTaskOptions(
[property: CommandSwitch("--destination-name")] string DestinationName,
[property: CommandSwitch("--sidewalk")] string Sidewalk
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--device-name")]
    public string? DeviceName { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}