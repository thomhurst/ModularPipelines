using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliCommand("hash-object")]
[ExcludeFromCodeCoverage]
public record GitHashObjectOptions : GitOptions
{
    [CliFlag("--stdin")]
    public virtual bool? Stdin { get; set; }

    [CliFlag("--stdin-paths")]
    public virtual bool? StdinPaths { get; set; }

    [CliFlag("--path")]
    public virtual bool? Path { get; set; }

    [CliFlag("--no-filters")]
    public virtual bool? NoFilters { get; set; }

    [CliFlag("--literally")]
    public virtual bool? Literally { get; set; }
}