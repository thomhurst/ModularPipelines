using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot-data", "delete-thing-shadow")]
public record AwsIotDataDeleteThingShadowOptions(
[property: CommandSwitch("--thing-name")] string ThingName
) : AwsOptions
{
    [CommandSwitch("--shadow-name")]
    public string? ShadowName { get; set; }
}