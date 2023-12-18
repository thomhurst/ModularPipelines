using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "copy", "run-command")]
public record AzStorageAzcopyRunCommandOptions : AzOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? CommandArgs { get; set; }
}

