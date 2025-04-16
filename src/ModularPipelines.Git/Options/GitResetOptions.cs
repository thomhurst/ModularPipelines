using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("reset")]
[ExcludeFromCodeCoverage]
public record GitResetOptions : GitOptions
{
    [BooleanCommandSwitch("--soft")]
    public virtual bool? Soft { get; set; }

    [BooleanCommandSwitch("--mixed")]
    public virtual bool? Mixed { get; set; }

    [BooleanCommandSwitch("--hard")]
    public virtual bool? Hard { get; set; }

    [BooleanCommandSwitch("--merge")]
    public virtual bool? Merge { get; set; }

    [BooleanCommandSwitch("--keep")]
    public virtual bool? Keep { get; set; }

    [BooleanCommandSwitch("--no-recurse-submodules")]
    public virtual bool? NoRecurseSubmodules { get; set; }

    [BooleanCommandSwitch("--recurse-submodules")]
    public virtual bool? RecurseSubmodules { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--refresh")]
    public virtual bool? Refresh { get; set; }

    [BooleanCommandSwitch("--no-refresh")]
    public virtual bool? NoRefresh { get; set; }

    [CommandEqualsSeparatorSwitch("--pathspec-from-file")]
    public string? PathspecFromFile { get; set; }

    [BooleanCommandSwitch("--pathspec-file-nul")]
    public virtual bool? PathspecFileNul { get; set; }
}