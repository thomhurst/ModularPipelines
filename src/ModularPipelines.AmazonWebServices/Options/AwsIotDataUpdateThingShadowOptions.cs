using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot-data", "update-thing-shadow")]
public record AwsIotDataUpdateThingShadowOptions(
[property: CliOption("--thing-name")] string ThingName,
[property: CliOption("--payload")] string Payload
) : AwsOptions
{
    [CliOption("--shadow-name")]
    public string? ShadowName { get; set; }
}