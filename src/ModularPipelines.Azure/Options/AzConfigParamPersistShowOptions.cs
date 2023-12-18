using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("config", "param-persist", "show")]
public record AzConfigParamPersistShowOptions : AzOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? NAME { get; set; }
}