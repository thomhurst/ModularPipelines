using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot-data", "delete-thing-shadow")]
public record AwsIotDataDeleteThingShadowOptions(
[property: CliOption("--thing-name")] string ThingName
) : AwsOptions
{
    [CliOption("--shadow-name")]
    public string? ShadowName { get; set; }
}