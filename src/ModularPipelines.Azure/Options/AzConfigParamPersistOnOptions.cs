using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("config", "param-persist", "on")]
public record AzConfigParamPersistOnOptions : AzOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? NAME { get; set; }
}