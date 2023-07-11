using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("switch")]
public record GitSwitchOptions : GitOptions
{
    [CommandEqualsSeparatorSwitch("--create")]
    public string? Create { get; set; }

    [CommandEqualsSeparatorSwitch("--force-create")]
    public string? ForceCreate { get; set; }

    [BooleanCommandSwitch("--detach")]
    public bool? Detach { get; set; }

    [BooleanCommandSwitch("--guess")]
    public bool? Guess { get; set; }

    [BooleanCommandSwitch("--no-guess")]
    public bool? NoGuess { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--discard-changes")]
    public bool? DiscardChanges { get; set; }

    [BooleanCommandSwitch("--merge")]
    public bool? Merge { get; set; }

    [CommandEqualsSeparatorSwitch("--conflict")]
    public string? Conflict { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--progress")]
    public bool? Progress { get; set; }

    [BooleanCommandSwitch("--no-progress")]
    public bool? NoProgress { get; set; }

    [BooleanCommandSwitch("--track")]
    public bool? Track { get; set; }

    [BooleanCommandSwitch("--no-track")]
    public bool? NoTrack { get; set; }

    [CommandEqualsSeparatorSwitch("--orphan")]
    public string? Orphan { get; set; }

    [BooleanCommandSwitch("--ignore-other-worktrees")]
    public bool? IgnoreOtherWorktrees { get; set; }

    [BooleanCommandSwitch("--recurse-submodules")]
    public bool? RecurseSubmodules { get; set; }

    [BooleanCommandSwitch("--no-recurse-submodules")]
    public bool? NoRecurseSubmodules { get; set; }
}