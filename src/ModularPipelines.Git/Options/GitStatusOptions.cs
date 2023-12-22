using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("status")]
[ExcludeFromCodeCoverage]
public record GitStatusOptions : GitOptions
{
    [BooleanCommandSwitch("--short")]
    public bool? Short { get; set; }

    [BooleanCommandSwitch("--branch")]
    public bool? Branch { get; set; }

    [BooleanCommandSwitch("--show-stash")]
    public bool? ShowStash { get; set; }

    [CommandEqualsSeparatorSwitch("--porcelain")]
    public string? Porcelain { get; set; }

    [BooleanCommandSwitch("--long")]
    public bool? Long { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public bool? Verbose { get; set; }

    [CommandEqualsSeparatorSwitch("--untracked-files")]
    public string? UntrackedFiles { get; set; }

    [CommandEqualsSeparatorSwitch("--ignore-submodules")]
    public string? IgnoreSubmodules { get; set; }

    [CommandEqualsSeparatorSwitch("--ignored")]
    public string? Ignored { get; set; }

    [CommandEqualsSeparatorSwitch("--column")]
    public string? Column { get; set; }

    [BooleanCommandSwitch("--no-column")]
    public bool? NoColumn { get; set; }

    [BooleanCommandSwitch("--ahead-behind")]
    public bool? AheadBehind { get; set; }

    [BooleanCommandSwitch("--no-ahead-behind")]
    public bool? NoAheadBehind { get; set; }

    [BooleanCommandSwitch("--renames")]
    public bool? Renames { get; set; }

    [BooleanCommandSwitch("--no-renames")]
    public bool? NoRenames { get; set; }

    [CommandEqualsSeparatorSwitch("--find-renames")]
    public string? FindRenames { get; set; }
}