using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("find")]
public record AzFindOptions : AzOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? CliTerm { get; set; }
}