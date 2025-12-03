using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "create-wireless-device")]
public record AwsIotwirelessCreateWirelessDeviceOptions(
[property: CliOption("--type")] string Type,
[property: CliOption("--destination-name")] string DestinationName
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--positioning")]
    public string? Positioning { get; set; }

    [CliOption("--sidewalk")]
    public string? Sidewalk { get; set; }

    [CliOption("--lorawan")]
    public string? Lorawan { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}