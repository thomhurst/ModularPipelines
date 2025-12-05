using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("set", "version")]
public record YarnSetVersionOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Version
) : YarnOptions
{
    [CliFlag("--yarn-path")]
    public virtual bool? YarnPath { get; set; }

    [CliFlag("--only-if-needed")]
    public virtual bool? OnlyIfNeeded { get; set; }
}