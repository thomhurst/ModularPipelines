using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("config", "unset")]
public record AzConfigUnsetOptions : AzOptions
{
    [BooleanCommandSwitch("--local")]
    public bool? Local { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? KEY { get; set; }
}