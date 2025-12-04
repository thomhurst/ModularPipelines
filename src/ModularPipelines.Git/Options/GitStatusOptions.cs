using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("status")]
[ExcludeFromCodeCoverage]
public record GitStatusOptions : GitOptions
{
    [CliFlag("--short")]
    public virtual bool? Short { get; set; }

    [CliFlag("--branch")]
    public virtual bool? Branch { get; set; }

    [CliFlag("--show-stash")]
    public virtual bool? ShowStash { get; set; }

    [CliOption("--porcelain", Format = OptionFormat.EqualsSeparated)]
    public string? Porcelain { get; set; }

    [CliFlag("--long")]
    public virtual bool? Long { get; set; }

    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }

    [CliOption("--untracked-files", Format = OptionFormat.EqualsSeparated)]
    public string? UntrackedFiles { get; set; }

    [CliOption("--ignore-submodules", Format = OptionFormat.EqualsSeparated)]
    public string? IgnoreSubmodules { get; set; }

    [CliOption("--ignored", Format = OptionFormat.EqualsSeparated)]
    public string? Ignored { get; set; }

    [CliOption("--column", Format = OptionFormat.EqualsSeparated)]
    public string? Column { get; set; }

    [CliFlag("--no-column")]
    public virtual bool? NoColumn { get; set; }

    [CliFlag("--ahead-behind")]
    public virtual bool? AheadBehind { get; set; }

    [CliFlag("--no-ahead-behind")]
    public virtual bool? NoAheadBehind { get; set; }

    [CliFlag("--renames")]
    public virtual bool? Renames { get; set; }

    [CliFlag("--no-renames")]
    public virtual bool? NoRenames { get; set; }

    [CliOption("--find-renames", Format = OptionFormat.EqualsSeparated)]
    public string? FindRenames { get; set; }
}