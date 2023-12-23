using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "update-wireless-device-import-task")]
public record AwsIotwirelessUpdateWirelessDeviceImportTaskOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--sidewalk")] string Sidewalk
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}