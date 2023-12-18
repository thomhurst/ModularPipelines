using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("config", "get")]
public record AzConfigGetOptions : AzOptions
{
    [BooleanCommandSwitch("--local")]
    public bool? Local { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? KEY { get; set; }
}