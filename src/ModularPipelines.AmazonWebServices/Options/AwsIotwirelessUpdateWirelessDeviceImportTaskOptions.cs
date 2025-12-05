using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "update-wireless-device-import-task")]
public record AwsIotwirelessUpdateWirelessDeviceImportTaskOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--sidewalk")] string Sidewalk
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}