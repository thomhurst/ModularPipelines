using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("config", "get")]
public record AzConfigGetOptions : AzOptions
{
    [BooleanCommandSwitch("--local")]
    public bool? Local { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Key { get; set; }
}