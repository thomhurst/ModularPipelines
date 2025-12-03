using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "get-wireless-device")]
public record AwsIotwirelessGetWirelessDeviceOptions(
[property: CliOption("--identifier")] string Identifier,
[property: CliOption("--identifier-type")] string IdentifierType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}