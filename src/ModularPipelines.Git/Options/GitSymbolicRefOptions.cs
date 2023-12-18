using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("symbolic-ref")]
[ExcludeFromCodeCoverage]
public record GitSymbolicRefOptions : GitOptions
{
    [BooleanCommandSwitch("--delete")]
    public bool? Delete { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--short")]
    public bool? Short { get; set; }

    [BooleanCommandSwitch("--recurse")]
    public bool? Recurse { get; set; }

    [BooleanCommandSwitch("--no-recurse")]
    public bool? NoRecurse { get; set; }
}