using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("config", "set")]
public record AzConfigSetOptions : AzOptions
{
    [BooleanCommandSwitch("--local")]
    public bool? Local { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? KEYVALUE { get; set; }
}