using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("status")]
[ExcludeFromCodeCoverage]
public record GitStatusOptions : GitOptions
{
    [BooleanCommandSwitch("--short")]
    public virtual bool? Short { get; set; }

    [BooleanCommandSwitch("--branch")]
    public virtual bool? Branch { get; set; }

    [BooleanCommandSwitch("--show-stash")]
    public virtual bool? ShowStash { get; set; }

    [CommandEqualsSeparatorSwitch("--porcelain")]
    public string? Porcelain { get; set; }

    [BooleanCommandSwitch("--long")]
    public virtual bool? Long { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public virtual bool? Verbose { get; set; }

    [CommandEqualsSeparatorSwitch("--untracked-files")]
    public string? UntrackedFiles { get; set; }

    [CommandEqualsSeparatorSwitch("--ignore-submodules")]
    public string? IgnoreSubmodules { get; set; }

    [CommandEqualsSeparatorSwitch("--ignored")]
    public string? Ignored { get; set; }

    [CommandEqualsSeparatorSwitch("--column")]
    public string? Column { get; set; }

    [BooleanCommandSwitch("--no-column")]
    public virtual bool? NoColumn { get; set; }

    [BooleanCommandSwitch("--ahead-behind")]
    public virtual bool? AheadBehind { get; set; }

    [BooleanCommandSwitch("--no-ahead-behind")]
    public virtual bool? NoAheadBehind { get; set; }

    [BooleanCommandSwitch("--renames")]
    public virtual bool? Renames { get; set; }

    [BooleanCommandSwitch("--no-renames")]
    public virtual bool? NoRenames { get; set; }

    [CommandEqualsSeparatorSwitch("--find-renames")]
    public string? FindRenames { get; set; }
}