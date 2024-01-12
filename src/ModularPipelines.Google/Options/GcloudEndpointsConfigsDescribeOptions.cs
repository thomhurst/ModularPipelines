using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("endpoints", "configs", "describe")]
public record GcloudEndpointsConfigsDescribeOptions(
[property: PositionalArgument] string ConfigId
) : GcloudOptions
{
    [CommandSwitch("--service")]
    public string? Service { get; set; }
}