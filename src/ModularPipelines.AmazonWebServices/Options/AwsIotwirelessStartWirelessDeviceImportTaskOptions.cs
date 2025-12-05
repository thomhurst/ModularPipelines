using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "start-wireless-device-import-task")]
public record AwsIotwirelessStartWirelessDeviceImportTaskOptions(
[property: CliOption("--destination-name")] string DestinationName,
[property: CliOption("--sidewalk")] string Sidewalk
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}