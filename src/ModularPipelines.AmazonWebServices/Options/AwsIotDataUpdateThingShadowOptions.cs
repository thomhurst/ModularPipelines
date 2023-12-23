using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot-data", "update-thing-shadow")]
public record AwsIotDataUpdateThingShadowOptions(
[property: CommandSwitch("--thing-name")] string ThingName,
[property: CommandSwitch("--payload")] string Payload
) : AwsOptions
{
    [CommandSwitch("--shadow-name")]
    public string? ShadowName { get; set; }
}