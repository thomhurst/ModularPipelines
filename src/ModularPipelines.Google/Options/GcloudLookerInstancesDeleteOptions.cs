using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("looker", "instances", "delete")]
public record GcloudLookerInstancesDeleteOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}