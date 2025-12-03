using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "copy", "run-command")]
public record AzStorageAzcopyRunCommandOptions : AzOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? COMMANDARGS { get; set; }
}