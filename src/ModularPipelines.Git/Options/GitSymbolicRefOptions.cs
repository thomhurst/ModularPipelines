using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("symbolic-ref")]
[ExcludeFromCodeCoverage]
public record GitSymbolicRefOptions : GitOptions
{
    [CliFlag("--delete")]
    public virtual bool? Delete { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--short")]
    public virtual bool? Short { get; set; }

    [CliFlag("--recurse")]
    public virtual bool? Recurse { get; set; }

    [CliFlag("--no-recurse")]
    public virtual bool? NoRecurse { get; set; }
}