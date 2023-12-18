using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("config", "param-persist", "delete")]
public record AzConfigParamPersistDeleteOptions : AzOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("--purge")]
    public bool? Purge { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? NAME { get; set; }
}