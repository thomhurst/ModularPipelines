using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "update-resource-position")]
public record AwsIotwirelessUpdateResourcePositionOptions(
[property: CliOption("--resource-identifier")] string ResourceIdentifier,
[property: CliOption("--resource-type")] string ResourceType
) : AwsOptions
{
    [CliOption("--geo-json-payload")]
    public string? GeoJsonPayload { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}