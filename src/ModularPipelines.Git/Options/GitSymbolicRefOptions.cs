using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("symbolic-ref")]
[ExcludeFromCodeCoverage]
public record GitSymbolicRefOptions : GitOptions
{
    [BooleanCommandSwitch("--delete")]
    public virtual bool? Delete { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--short")]
    public virtual bool? Short { get; set; }

    [BooleanCommandSwitch("--recurse")]
    public virtual bool? Recurse { get; set; }

    [BooleanCommandSwitch("--no-recurse")]
    public virtual bool? NoRecurse { get; set; }
}