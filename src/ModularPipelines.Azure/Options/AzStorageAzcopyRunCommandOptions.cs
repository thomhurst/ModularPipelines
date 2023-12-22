using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "copy", "run-command")]
public record AzStorageAzcopyRunCommandOptions : AzOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? COMMANDARGS { get; set; }
}