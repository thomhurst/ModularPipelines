using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("up")]
public record YarnUpOptions : YarnOptions
{
    [CliFlag("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CliFlag("--fixed")]
    public virtual bool? Fixed { get; set; }

    [CliFlag("--exact")]
    public virtual bool? Exact { get; set; }

    [CliFlag("--tilde")]
    public virtual bool? Tilde { get; set; }

    [CliFlag("--caret")]
    public virtual bool? Caret { get; set; }

    [CliFlag("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CliOption("--mode")]
    public virtual string? Mode { get; set; }
}