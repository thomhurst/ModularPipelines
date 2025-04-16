using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("hash-object")]
[ExcludeFromCodeCoverage]
public record GitHashObjectOptions : GitOptions
{
    [BooleanCommandSwitch("--stdin")]
    public virtual bool? Stdin { get; set; }

    [BooleanCommandSwitch("--stdin-paths")]
    public virtual bool? StdinPaths { get; set; }

    [BooleanCommandSwitch("--path")]
    public virtual bool? Path { get; set; }

    [BooleanCommandSwitch("--no-filters")]
    public virtual bool? NoFilters { get; set; }

    [BooleanCommandSwitch("--literally")]
    public virtual bool? Literally { get; set; }
}