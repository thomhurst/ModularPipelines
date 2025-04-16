using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("switch")]
[ExcludeFromCodeCoverage]
public record GitSwitchOptions : GitOptions
{
    [CommandEqualsSeparatorSwitch("--create")]
    public string? Create { get; set; }

    [CommandEqualsSeparatorSwitch("--force-create")]
    public string? ForceCreate { get; set; }

    [BooleanCommandSwitch("--detach")]
    public virtual bool? Detach { get; set; }

    [BooleanCommandSwitch("--guess")]
    public virtual bool? Guess { get; set; }

    [BooleanCommandSwitch("--no-guess")]
    public virtual bool? NoGuess { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [BooleanCommandSwitch("--discard-changes")]
    public virtual bool? DiscardChanges { get; set; }

    [BooleanCommandSwitch("--merge")]
    public virtual bool? Merge { get; set; }

    [CommandEqualsSeparatorSwitch("--conflict")]
    public string? Conflict { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--progress")]
    public virtual bool? Progress { get; set; }

    [BooleanCommandSwitch("--no-progress")]
    public virtual bool? NoProgress { get; set; }

    [BooleanCommandSwitch("--track")]
    public virtual bool? Track { get; set; }

    [BooleanCommandSwitch("--no-track")]
    public virtual bool? NoTrack { get; set; }

    [CommandEqualsSeparatorSwitch("--orphan")]
    public string? Orphan { get; set; }

    [BooleanCommandSwitch("--ignore-other-worktrees")]
    public virtual bool? IgnoreOtherWorktrees { get; set; }

    [BooleanCommandSwitch("--recurse-submodules")]
    public virtual bool? RecurseSubmodules { get; set; }

    [BooleanCommandSwitch("--no-recurse-submodules")]
    public virtual bool? NoRecurseSubmodules { get; set; }
}