using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("switch")]
[ExcludeFromCodeCoverage]
public record GitSwitchOptions : GitOptions
{
    [CliOption("--create", Format = OptionFormat.EqualsSeparated)]
    public string? Create { get; set; }

    [CliOption("--force-create", Format = OptionFormat.EqualsSeparated)]
    public string? ForceCreate { get; set; }

    [CliFlag("--detach")]
    public virtual bool? Detach { get; set; }

    [CliFlag("--guess")]
    public virtual bool? Guess { get; set; }

    [CliFlag("--no-guess")]
    public virtual bool? NoGuess { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliFlag("--discard-changes")]
    public virtual bool? DiscardChanges { get; set; }

    [CliFlag("--merge")]
    public virtual bool? Merge { get; set; }

    [CliOption("--conflict", Format = OptionFormat.EqualsSeparated)]
    public string? Conflict { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--progress")]
    public virtual bool? Progress { get; set; }

    [CliFlag("--no-progress")]
    public virtual bool? NoProgress { get; set; }

    [CliFlag("--track")]
    public virtual bool? Track { get; set; }

    [CliFlag("--no-track")]
    public virtual bool? NoTrack { get; set; }

    [CliOption("--orphan", Format = OptionFormat.EqualsSeparated)]
    public string? Orphan { get; set; }

    [CliFlag("--ignore-other-worktrees")]
    public virtual bool? IgnoreOtherWorktrees { get; set; }

    [CliFlag("--recurse-submodules")]
    public virtual bool? RecurseSubmodules { get; set; }

    [CliFlag("--no-recurse-submodules")]
    public virtual bool? NoRecurseSubmodules { get; set; }
}